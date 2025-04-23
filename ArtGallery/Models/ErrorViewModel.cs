namespace ArtGallery.Models;

// Modèle utilisé pour afficher les erreurs dans l'application.
public class ErrorViewModel
{
    // Identifiant de la requête (pour le suivi des erreurs).
    public string? RequestId { get; set; }

    // Indique si l'identifiant de la requête doit être affiché.
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}
