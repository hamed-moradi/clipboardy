using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Domain.Configurations {
  public class ContentTypeConfiguration {
    internal static void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.Entity<ContentType>()
        .HasIndex(p => p.extension)
        .IsUnique();

      modelBuilder.Entity<ContentType>().HasData(
        new ContentType { id = 1, extension = ".aac", name = "AAC audio", mime_type = "audio/aac" },
        new ContentType { id = 2, extension = ".abw", name = "AbiWord document", mime_type = "application/x-abiword" },
        new ContentType { id = 3, extension = ".arc", name = "Archive document (multiple files embedded)", mime_type = "application/x-freearc" },
        new ContentType { id = 4, extension = ".avif", name = "AVIF image", mime_type = "image/avif" },
        new ContentType { id = 5, extension = ".avi", name = "AVI: Audio Video Interleave", mime_type = "video/x-msvideo" },
        new ContentType { id = 6, extension = ".azw", name = "Amazon Kindle eBook format", mime_type = "application/vnd.amazon.ebook" },
        new ContentType { id = 7, extension = ".bin", name = "Any kind of binary data", mime_type = "application/octet-stream" },
        new ContentType { id = 8, extension = ".bmp", name = "Windows OS/2 Bitmap Graphics", mime_type = "image/bmp" },
        new ContentType { id = 9, extension = ".csh", name = "C-Shell script", mime_type = "application/x-csh" },
        new ContentType { id = 10, extension = ".css", name = "Cascading Style Sheets (CSS)", mime_type = "text/css" },
        new ContentType { id = 11, extension = ".csv ", name = "Comma - separated values(CSV)", mime_type = "text/csv" },
        new ContentType { id = 12, extension = ".doc ", name = "Microsoft Word", mime_type = "application/msword" },
        new ContentType { id = 13, extension = ".docx", name = "Microsoft Word (OpenXML)", mime_type = "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
        new ContentType { id = 14, extension = ".eot ", name = "MS Embedded OpenType fonts", mime_type = "application/vnd.ms-fontobject" },
        new ContentType { id = 15, extension = ".epub", name = "Electronic publication (EPUB)", mime_type = "application/epub+zip" },
        new ContentType { id = 16, extension = ".gz", name = "GZip Compressed Archive", mime_type = "application/gzip" },
        new ContentType { id = 17, extension = ".gif", name = "Graphics Interchange Format (GIF)", mime_type = "image/gif" },
        new ContentType { id = 18, extension = ".ht", name = "HyperText Markup Language (HTML)", mime_type = "text/html" },
        new ContentType { id = 19, extension = ".ico ", name = "Icon format", mime_type = "image/vnd.microsoft.icon" },
        new ContentType { id = 20, extension = ".ics", name = "iCalendar format", mime_type = "text/calendar" },
        new ContentType { id = 21, extension = ".jar", name = "Java Archive (JAR)", mime_type = "application/java-archive" },
        new ContentType { id = 22, extension = ".jpe", name = "JPEG images", mime_type = "image/jpeg" },
        new ContentType { id = 23, extension = ".js", name = "JavaScript", mime_type = "text/javascript" },
        new ContentType { id = 24, extension = ".json", name = "JSON format", mime_type = "application/json" },
        new ContentType { id = 25, extension = ".jsonld", name = "JSON-LD format", mime_type = "application/ld+json" },
        new ContentType { id = 26, extension = ".mi", name = "Musical Instrument Digital Interface (MIDI)", mime_type = "audio/midi" },
        new ContentType { id = 27, extension = ".midi", name = "Musical Instrument Digital Interface (MIDI)", mime_type = "audio/x-midi" },
        new ContentType { id = 28, extension = ".mjs ", name = "JavaScript module", mime_type = "text/javascript" },
        new ContentType { id = 29, extension = ".mp3", name = "MP3 audio", mime_type = "audio/mpeg" },
        new ContentType { id = 30, extension = ".mp4", name = "MP4 video", mime_type = "video/mp4" },
        new ContentType { id = 31, extension = ".mpeg", name = "MPEG Video", mime_type = "video/mpeg" },
        new ContentType { id = 32, extension = ".mpkg", name = "Apple Installer Package", mime_type = "application/vnd.apple.installer+xml" },
        new ContentType { id = 33, extension = ".odp", name = "OpenDocument presentation document", mime_type = "application/vnd.oasis.opendocument.presentation" },
        new ContentType { id = 34, extension = ".ods", name = "OpenDocument spreadsheet document", mime_type = "application/vnd.oasis.opendocument.spreadsheet" },
        new ContentType { id = 35, extension = ".odt", name = "OpenDocument text document", mime_type = "application/vnd.oasis.opendocument.text" },
        new ContentType { id = 36, extension = ".oga", name = "OGG audio", mime_type = "audio/ogg" },
        new ContentType { id = 37, extension = ".ogv", name = "OGG video", mime_type = "video/ogg" },
        new ContentType { id = 38, extension = ".ogx", name = "OGG", mime_type = "application/ogg" },
        new ContentType { id = 39, extension = ".opus", name = "Opus audio", mime_type = "audio/opus" },
        new ContentType { id = 40, extension = ".otf", name = "OpenType font", mime_type = "font/otf" },
        new ContentType { id = 41, extension = ".png", name = "Portable Network Graphics", mime_type = "image/png" },
        new ContentType { id = 42, extension = ".pdf", name = "Apple Installer Package", mime_type = "application/vnd.apple.installer+xml" },
        new ContentType { id = 43, extension = ".php ", name = "OpenDocument text document", mime_type = "application/vnd.oasis.opendocument.text" },
        new ContentType { id = 44, extension = ".ppt ", name = "OGG audio", mime_type = "audio/ogg" },
        new ContentType { id = 45, extension = ".pptx", name = "Microsoft PowerPoint (OpenXML)", mime_type = "application/vnd.openxmlformats-officedocument.presentationml.presentation" },
        new ContentType { id = 46, extension = ".rar ", name = "RAR archive", mime_type = "application/vnd.rar" },
        new ContentType { id = 47, extension = ".rtf", name = "Rich Text Format (RTF)", mime_type = "application/rtf" },
        new ContentType { id = 48, extension = ".sh", name = "Bourne shell script", mime_type = "application/x-sh" },
        new ContentType { id = 49, extension = ".svg", name = "Scalable Vector Graphics (SVG)", mime_type = "image/svg+xml" },
        new ContentType { id = 50, extension = ".tar ", name = "Tape Archive(TAR)", mime_type = "application/x-tar" },
        new ContentType { id = 51, extension = ".ti", name = "Tagged Image File Format (TIFF)", mime_type = "image/tiff" },
        new ContentType { id = 52, extension = ".ts", name = "MPEG transport stream", mime_type = "video/mp2t" },
        new ContentType { id = 53, extension = ".ttf", name = "TrueType Font", mime_type = "font/ttf" },
        new ContentType { id = 54, extension = ".txt", name = "Text, (generally ASCII or ISO 8859 - n)", mime_type = "text/plain" },
        new ContentType { id = 55, extension = ".vsd", name = "Microsoft Visio", mime_type = "application/vnd.visio" },
        new ContentType { id = 56, extension = ".wav", name = "Waveform Audio Format", mime_type = "audio/wav" },
        new ContentType { id = 57, extension = ".weba", name = "WEBM audio", mime_type = "audio/webm" },
        new ContentType { id = 58, extension = ".webm", name = "WEBM video", mime_type = "video/webm" },
        new ContentType { id = 59, extension = ".webp", name = "WEBP image", mime_type = "image/webp" },
        new ContentType { id = 60, extension = ".woff", name = "Web Open Font Format (WOFF)", mime_type = "font/woff" },
        new ContentType { id = 61, extension = ".woff2 ", name = "Web Open Font Format (WOFF)", mime_type = "font/woff2" },
        new ContentType { id = 62, extension = ".xhtml ", name = "XHTML", mime_type = "application/xhtml+xml" },
        new ContentType { id = 63, extension = ".xls", name = "Microsoft Excel", mime_type = "application/vnd.ms-excel" },
        new ContentType { id = 64, extension = ".xlsx", name = "Microsoft Excel (OpenXML)", mime_type = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
        new ContentType { id = 65, extension = ".xml ", name = "XML", mime_type = "application/xml" },
        new ContentType { id = 66, extension = ".xul", name = "XUL", mime_type = "application/vnd.mozilla.xul+xml" },
        new ContentType { id = 67, extension = ".zip", name = "ZIP archive", mime_type = "application/zip" },
        new ContentType { id = 68, extension = ".7z", name = "7-zip archive", mime_type = "application/x-7z-compressed" },
        new ContentType { id = 69, extension = ".htm", name = "HyperText Markup Language (HTML)", mime_type = "text/html" },
        new ContentType { id = 70, extension = ".html", name = "HyperText Markup Language (HTML)", mime_type = "text/html" });
    }
  }
}


