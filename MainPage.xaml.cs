using System.Globalization;

namespace Calculatrice;

public partial class MainPage : ContentPage
{
    // Valeur en cours de saisie (point décimal en interne, virgule à l'affichage)
    string saisie = "0";

    // Premier nombre et opérateur en attente
    double? premierOperande = null;
    string? operateur = null;

    // Vrai juste après un opérateur ou un résultat : le prochain chiffre repart de zéro
    bool nouvelleSaisie = false;

    // Vrai quand une erreur est affichée (division par zéro, dépassement)
    bool enErreur = false;

    const int LongueurMax = 12;

    public MainPage()
    {
        InitializeComponent();
    }

    // ---------- Saisie des chiffres ----------

    void OnChiffreClicked(object sender, EventArgs e)
    {
        if (enErreur) Reinitialiser();

        string chiffre = ((Button)sender).Text;

        if (nouvelleSaisie)
        {
            saisie = "0";
            nouvelleSaisie = false;
        }

        // On limite la longueur pour que l'affichage reste lisible
        if (saisie.Replace("-", "").Replace(".", "").Length >= LongueurMax) return;

        if (saisie == "0") saisie = chiffre;
        else if (saisie == "-0") saisie = "-" + chiffre;
        else saisie += chiffre;

        MettreAJourAffichage();
    }

    void OnVirguleClicked(object sender, EventArgs e)
    {
        if (enErreur) Reinitialiser();

        if (nouvelleSaisie)
        {
            saisie = "0";
            nouvelleSaisie = false;
        }

        // Un seul séparateur décimal par nombre
        if (saisie.Contains('.')) return;

        saisie += ".";
        MettreAJourAffichage();
    }

    // ---------- Opérateurs et calcul ----------

    void OnOperateurClicked(object sender, EventArgs e)
    {
        if (enErreur) return;

        string op = ((Button)sender).Text;
        double courant = double.Parse(saisie, CultureInfo.InvariantCulture);

        if (premierOperande == null)
        {
            premierOperande = courant;
        }
        else if (!nouvelleSaisie && operateur != null)
        {
            // Enchaînement (ex : 2 + 3 puis ×) : on calcule d'abord l'opération en attente
            double? resultat = Calculer(premierOperande.Value, courant, operateur);
            if (resultat == null)
            {
                AfficherErreur(operateur == "÷" && courant == 0 ? "Division par zéro" : "Résultat trop grand");
                return;
            }
            premierOperande = resultat;
            saisie = Formater(resultat.Value);
        }
        // Sinon : l'utilisateur a appuyé deux opérateurs de suite, on remplace simplement l'opérateur

        operateur = op;
        nouvelleSaisie = true;

        LabelExpression.Text = $"{Formater(premierOperande.Value)} {operateur}";
        MettreAJourAffichage();
    }

    void OnEgalClicked(object sender, EventArgs e)
    {
        if (enErreur || premierOperande == null || operateur == null) return;

        // Opérateur saisi mais pas de second nombre : rien à calculer
        if (nouvelleSaisie) return;

        double courant = double.Parse(saisie, CultureInfo.InvariantCulture);
        double? resultat = Calculer(premierOperande.Value, courant, operateur);

        if (resultat == null)
        {
            AfficherErreur(operateur == "÷" && courant == 0 ? "Division par zéro" : "Résultat trop grand");
            return;
        }

        LabelExpression.Text = $"{Formater(premierOperande.Value)} {operateur} {Formater(courant)} =";
        saisie = Formater(resultat.Value);
        premierOperande = null;
        operateur = null;
        nouvelleSaisie = true;

        MettreAJourAffichage();
    }

    // Renvoie null si le calcul est impossible
    double? Calculer(double a, double b, string op)
    {
        double r;
        switch (op)
        {
            case "+": r = a + b; break;
            case "-": r = a - b; break;
            case "×": r = a * b; break;
            case "÷":
                if (b == 0) return null;
                r = a / b;
                break;
            default: return null;
        }

        if (double.IsNaN(r) || double.IsInfinity(r)) return null;
        return r;
    }

    // ---------- Boutons d'action ----------

    void OnEffacerClicked(object sender, EventArgs e)
    {
        Reinitialiser();
    }

    void OnSupprimerClicked(object sender, EventArgs e)
    {
        if (enErreur || nouvelleSaisie) return;

        saisie = saisie.Substring(0, saisie.Length - 1);
        if (saisie == "" || saisie == "-") saisie = "0";

        MettreAJourAffichage();
    }

    void OnSigneClicked(object sender, EventArgs e)
    {
        if (enErreur || saisie == "0") return;

        saisie = saisie.StartsWith("-") ? saisie.Substring(1) : "-" + saisie;
        MettreAJourAffichage();
    }

    // ---------- Utilitaires ----------

    void Reinitialiser()
    {
        saisie = "0";
        premierOperande = null;
        operateur = null;
        nouvelleSaisie = false;
        enErreur = false;
        LabelExpression.Text = "";
        MettreAJourAffichage();
    }

    void AfficherErreur(string message)
    {
        enErreur = true;
        premierOperande = null;
        operateur = null;
        LabelExpression.Text = message;
        LabelAffichage.Text = "Erreur";
        LabelAffichage.FontSize = 40;
    }

    // Arrondi pour éviter les 0,30000000000000004 des nombres à virgule flottante
    string Formater(double valeur)
    {
        return Math.Round(valeur, 10).ToString("G12", CultureInfo.InvariantCulture);
    }

    void MettreAJourAffichage()
    {
        LabelAffichage.Text = saisie.Replace('.', ',');
        LabelAffichage.FontSize = saisie.Length > 9 ? 36 : 56;
    }
}