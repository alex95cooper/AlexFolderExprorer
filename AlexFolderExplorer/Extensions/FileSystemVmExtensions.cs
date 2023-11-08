using System.Xml.Linq;
using AlexFolderExplorer.ViewModels;

namespace AlexFolderExplorer.Extensions;

public static class FileSystemVmExtensions
{
    public static XElement ToXElement(this FileSystemViewModel model, string systemElementType)
    {
        return new XElement(systemElementType,
            new XAttribute("name", model.Name),
            new XAttribute("size", model.Size),
            new XAttribute("creationDate", model.CreationDate.ToShortDateString()),
            new XAttribute("lastAccessDate", model.LastAccessDate.ToShortDateString()),
            new XAttribute("lastModifiedDate", model.ModifiedDate.ToShortDateString()),
            new XAttribute("attributes", model.Attributes), new XAttribute("owner", model.Owner),
            new XAttribute("rights", model.Rights));
    }
}