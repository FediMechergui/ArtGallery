# ArtGallery - Site de Galerie et Vente d'Art

Ce projet est un site web pour un artiste permettant de présenter et vendre ses créations artistiques (peintures, sculptures, home déco, etc.).

## Aperçu du Projet

ArtGallery est une application web développée avec ASP.NET Core MVC en C# et utilisant SQL Server comme base de données. Cette plateforme permet à un artiste de présenter son travail, interagir avec sa communauté et vendre ses œuvres en ligne.

## Technologies Utilisées

- **Backend**: ASP.NET Core MVC en C#
- **Base de données**: SQL Server
- **Frontend**: HTML5, CSS3, JavaScript
- **Stockage des images**: Stockage local pour les images des œuvres d'art
- **Authentification**: ASP.NET Core Identity

## Fonctionnalités

- **Biographie de l'artiste**: Section dédiée à la présentation de l'artiste
- **Présentation des créations**: Affichage des œuvres avec description des techniques utilisées
- **Galerie photos**: Présentation visuelle des collections et expositions
- **Livre d'or**: Espace de discussion permettant aux visiteurs de laisser des commentaires et partager des idées
- **Liens externes**: Redirection vers les sites d'autres artistes et lieux d'exposition
- **Boutique en ligne**: Espace de vente organisé par catégories d'œuvres
- **Gestion des commandes**: Système permettant à l'artiste d'approuver les commandes

## Structure du Projet

```
ArtGallery/
├─ AppData/
├─ bin/
├─ Controllers/
│  ├─ HomeController.cs
│  ├─ AccountController.cs
│  ├─ GalleryController.cs
│  ├─ ShopController.cs
│  ├─ AdminController.cs
│  └─ GoldenBookController.cs
├─ Migrations/
├─ Models/
│  ├─ Artwork.cs
│  ├─ Category.cs
│  ├─ Exhibition.cs
│  ├─ GoldenBookEntry.cs
│  ├─ Order.cs
│  ├─ ExternalLink.cs
│  └─ ViewModels/
├─ obj/
├─ Properties/
├─ Views/
│  ├─ Home/
│  │  ├─ Index.cshtml
│  │  ├─ Biography.cshtml
│  │  └─ Contact.cshtml
│  ├─ Gallery/
│  │  ├─ Index.cshtml
│  │  ├─ Collection.cshtml
│  │  └─ Exhibition.cshtml
│  ├─ Shop/
│  │  ├─ Index.cshtml
│  │  ├─ Category.cshtml
│  │  ├─ Detail.cshtml
│  │  └─ Checkout.cshtml
│  ├─ GoldenBook/
│  │  ├─ Index.cshtml
│  │  └─ Create.cshtml
│  ├─ Admin/
│  │  ├─ Dashboard.cshtml
│  │  ├─ ArtworkManagement.cshtml
│  │  ├─ OrderManagement.cshtml
│  │  └─ GoldenBookModeration.cshtml
│  ├─ Shared/
│  │  ├─ _Layout.cshtml
│  │  └─ _Navigation.cshtml
│  └─ _ViewImports.cshtml
├─ wwwroot/
│  ├─ css/
│  ├─ js/
│  ├─ lib/
│  └─ images/
│     ├─ artworks/
│     └─ exhibitions/
├─ appsettings.Development.json
├─ appsettings.json
├─ Program.cs
└─ ArtGallery.csproj
```

## Installation et Configuration

### Prérequis

- Visual Studio 2022 ou plus récent
- .NET 8.0 SDK
- SQL Server

### Étapes d'Installation

1. Cloner le dépôt Git
2. Ouvrir la solution dans Visual Studio
3. Restaurer les packages NuGet
4. Exécuter le script `schema.sql` sur votre instance SQL Server pour créer la base de données
5. Modifier les chaînes de connexion dans `appsettings.json` selon votre configuration
6. Exécuter les migrations: `Update-Database` dans la console Package Manager
7. Lancer l'application avec F5 ou en cliquant sur le bouton "Exécuter"

## Architecture

L'application suit le modèle MVC (Modèle-Vue-Contrôleur) d'ASP.NET Core:

- **Modèles**: Représentent les données de l'application (œuvres d'art, catégories, commandes, etc.)
- **Vues**: Interfaces utilisateur pour afficher les données
- **Contrôleurs**: Gèrent les requêtes des utilisateurs et coordonnent les actions

### Rôles d'Utilisateur

- **Administrateur (Artiste)**: Accès complet au système, peut gérer le contenu et approuver les commandes
- **Visiteurs**: Peuvent parcourir le site, voir les œuvres et laisser des commentaires
- **Clients**: Peuvent acheter des œuvres d'art

## Fonctionnement de l'Application

1. L'artiste (administrateur) se connecte et peut ajouter/gérer ses œuvres
2. Les visiteurs peuvent parcourir la galerie et les informations sur l'artiste
3. Les utilisateurs peuvent laisser des commentaires dans le livre d'or
4. Les clients peuvent passer des commandes qui seront soumises à l'approbation de l'artiste

## Développement Futur

- Intégration de passerelles de paiement
- Système de notifications pour les nouvelles commandes
- Interface administrateur améliorée pour la gestion des contenus
- Analytics pour suivre les performances du site