Calculatrice .NET MAUI

Application de calculatrice développée avec .NET MAUI dans le cadre de l'Activité N°4 de l'atelier de développement mobile.

Auteur : NGOK FILS ARNOLD

Objectif

Mettre en pratique la prise en main de .NET MAUI : structure d'un projet Single Project, interface construite en XAML, dispositions (layouts) et liaison entre le XAML et le code-behind en C#. L'accent est mis autant sur la qualité de la disposition que sur le bon fonctionnement des calculs.

Fonctionnalités

Opérations de base

Addition, soustraction, multiplication et division, avec enchaînement des opérations (2 + 3 × 4 est calculé de gauche à droite).
Saisie des nombres décimaux (un seul séparateur décimal par nombre).
C : remise à zéro totale.
⌫ : effacement du dernier caractère saisi.
± : changement de signe.
% : pourcentage. Avec + ou -, le pourcentage porte sur le premier nombre (200 + 10 % donne 220) ; sinon le nombre est divisé par 100.
Affichage de l'opération en cours au-dessus du résultat.

Fonctions avancées

x² : carré.
√ : racine carrée.
1/x : inverse.

Cas limites et erreurs gérés

Division par zéro : l'application affiche « Erreur » et un message, sans planter.
Racine carrée d'un nombre négatif et résultats trop grands : même traitement.
Un seul séparateur décimal par nombre, pas de zéros inutiles en début de nombre.
Longueur de saisie limitée à 12 chiffres pour que l'affichage reste lisible.
Deux opérateurs de suite : le second remplace le premier.
Les erreurs d'arrondi des nombres à virgule flottante sont corrigées (0,1 + 0,2 donne 0,3).
Dispositions (layouts) utilisées
Layout	Où	Pourquoi
Grid	Page entière (3 lignes) et clavier (5 × 4)	Ses lignes et colonnes proportionnelles remplissent tout l'écran et gardent les touches alignées, quelle que soit la taille.
Border	Autour de l'affichage	Elle forme une carte arrondie qui met l'affichage en évidence et le sépare du clavier.
VerticalStackLayout	Dans la carte d'affichage	Il empile l'opération en cours au-dessus du résultat sans calcul de positions.
ScrollView (horizontal)	Autour de l'opération en cours	Une longue expression défile au lieu de déborder de l'écran.
FlexLayout	Rangée x² / √ / 1/x	Les trois touches se partagent la largeur disponible, en portrait comme en paysage.

Adaptation à l'écran : la taille du résultat diminue quand la hauteur disponible est faible (paysage ou petit écran) et quand le nombre devient long. Le clavier est dans une Grid à lignes et colonnes proportionnelles, donc les touches se redimensionnent avec la fenêtre.

Structure du projet
Calculatrice/
├── MainPage.xaml       # Interface : layouts, affichage et touches
├── MainPage.xaml.cs    # Logique : gestionnaires d'événements et calculs
├── App.xaml / App.xaml.cs
├── AppShell.xaml / AppShell.xaml.cs
├── MauiProgram.cs
├── Platforms/          # Code spécifique à chaque plateforme
├── Resources/          # Icônes, polices, images
└── Calculatrice.csproj
Prérequis
SDK .NET 10
Charge de travail MAUI pour Windows :
bash
dotnet workload install maui-windows
Lancer l'application

Depuis le dossier du projet :

bash
dotnet restore
dotnet run -f net10.0-windows10.0.19041.0

Sous Windows, le mode développeur doit être activé (Paramètres, puis « Confidentialité et sécurité », puis « Pour les développeurs »). La première restauration des paquets peut être longue selon la connexion.

Cible actuelle et Android

Le projet cible pour l'instant Windows (net10.0-windows10.0.19041.0) dans le fichier Calculatrice.csproj. Pour ajouter Android, il faut installer la charge de travail maui-android, le JDK et l'Android SDK, puis remplacer la ligne TargetFrameworks par :

xml
<TargetFrameworks>net10.0-android;net10.0-windows10.0.19041.0</TargetFrameworks>

L'application n'a pas encore été testée sur Android.

Ressources
Documentation officielle .NET MAUI
Documentation des layouts
