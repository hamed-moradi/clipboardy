using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Core.Domain.Migrations {
  /// <inheritdoc />
  public partial class ContentTypeData: Migration {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder) {
      migrationBuilder.InsertData(
          table: "content_type",
          columns: new[] { "id", "extension", "mime_type", "name" },
          values: new object[,]
          {
                    { 1, ".aac", "audio/aac", "AAC audio" },
                    { 2, ".abw", "application/x-abiword", "AbiWord document" },
                    { 3, ".arc", "application/x-freearc", "Archive document (multiple files embedded)" },
                    { 4, ".avif", "image/avif", "AVIF image" },
                    { 5, ".avi", "video/x-msvideo", "AVI: Audio Video Interleave" },
                    { 6, ".azw", "application/vnd.amazon.ebook", "Amazon Kindle eBook format" },
                    { 7, ".bin", "application/octet-stream", "Any kind of binary data" },
                    { 8, ".bmp", "image/bmp", "Windows OS/2 Bitmap Graphics" },
                    { 9, ".csh", "application/x-csh", "C-Shell script" },
                    { 10, ".css", "text/css", "Cascading Style Sheets (CSS)" },
                    { 11, ".csv ", "text/csv", "Comma - separated values(CSV)" },
                    { 12, ".doc ", "application/msword", "Microsoft Word" },
                    { 13, ".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "Microsoft Word (OpenXML)" },
                    { 14, ".eot ", "application/vnd.ms-fontobject", "MS Embedded OpenType fonts" },
                    { 15, ".epub", "application/epub+zip", "Electronic publication (EPUB)" },
                    { 16, ".gz", "application/gzip", "GZip Compressed Archive" },
                    { 17, ".gif", "image/gif", "Graphics Interchange Format (GIF)" },
                    { 18, ".ht", "text/html", ", .html HyperText Markup Language (HTML)" },
                    { 19, ".ico ", "image/vnd.microsoft.icon", "Icon format" },
                    { 20, ".ics", "text/calendar", "iCalendar format" },
                    { 21, ".jar", "application/java-archive", "Java Archive (JAR)" },
                    { 22, ".jpe", "image/jpeg", ", .jpg JPEG images" },
                    { 23, ".js", "text/javascript", "JavaScript" },
                    { 24, ".json", "application/json", "JSON format" },
                    { 25, ".jsonld", "application/ld+json", "JSON-LD format" },
                    { 26, ".mi", "audio/midi", "	Musical Instrument Digital Interface (MIDI)" },
                    { 27, ".midi", "audio/x-midi", "	Musical Instrument Digital Interface (MIDI)" },
                    { 28, ".mjs ", "text/javascript", "JavaScript module" },
                    { 29, ".mp3", "audio/mpeg", "MP3 audio" },
                    { 30, ".mp4", "video/mp4", "MP4 video" },
                    { 31, ".mpeg", "video/mpeg", "MPEG Video" },
                    { 32, ".mpkg", "application/vnd.apple.installer+xml", "Apple Installer Package" },
                    { 33, ".odp", "application/vnd.oasis.opendocument.presentation", "OpenDocument presentation document" },
                    { 34, ".ods", "application/vnd.oasis.opendocument.spreadsheet", "OpenDocument spreadsheet document" },
                    { 35, ".odt", "application/vnd.oasis.opendocument.text", "OpenDocument text document" },
                    { 36, ".oga", "audio/ogg", "OGG audio" },
                    { 37, ".ogv", "video/ogg", "OGG video" },
                    { 38, ".ogx", "application/ogg", "OGG" },
                    { 39, ".opus", "audio/opus", "Opus audio" },
                    { 40, ".otf", "font/otf", "OpenType font" },
                    { 41, ".png", "image/png", "Portable Network Graphics" },
                    { 42, ".pdf", "application/vnd.apple.installer+xml", "Apple Installer Package" },
                    { 43, ".php ", "application/vnd.oasis.opendocument.text", "OpenDocument text document" },
                    { 44, ".ppt ", "audio/ogg", "OGG audio" },
                    { 45, ".pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation", "Microsoft PowerPoint (OpenXML)" },
                    { 46, ".rar ", "application/vnd.rar", "RAR archive" },
                    { 47, ".rtf", "application/rtf", "Rich Text Format (RTF)" },
                    { 48, ".sh", "application/x-sh", "Bourne shell script" },
                    { 49, ".svg", "image/svg+xml", "Scalable Vector Graphics (SVG)" },
                    { 50, ".tar ", "application/x-tar", "Tape Archive(TAR)" },
                    { 51, ".ti", "image/tiff", ", .tiff Tagged Image File Format (TIFF)" },
                    { 52, ".ts", "video/mp2t", "MPEG transport stream" },
                    { 53, ".ttf", "font/ttf", "TrueType Font" },
                    { 54, ".txt", "text/plain", "Text, (generally ASCII or ISO 8859 - n)" },
                    { 55, ".vsd", "application/vnd.visio", "Microsoft Visio" },
                    { 56, ".wav", "audio/wav", "Waveform Audio Format" },
                    { 57, ".weba", "audio/webm", "WEBM audio" },
                    { 58, ".webm", "video/webm", "WEBM video" },
                    { 59, ".webp", "image/webp", "WEBP image" },
                    { 60, ".woff", "font/woff", "Web Open Font Format (WOFF)" },
                    { 61, ".woff2 ", "font/woff2", "Web Open Font Format (WOFF)" },
                    { 62, ".xhtml ", "application/xhtml+xml", "XHTML" },
                    { 63, ".xls", "application/vnd.ms-excel", "Microsoft Excel" },
                    { 64, ".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Microsoft Excel (OpenXML)" },
                    { 65, ".xml ", "application/xml", "XML" },
                    { 66, ".xul", "application/vnd.mozilla.xul+xml", "XUL" },
                    { 67, ".zip", "application/zip", "ZIP archive" },
                    { 68, ".7z", "application/x-7z-compressed", "7-zip archive" }
          });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder) {
      migrationBuilder.DeleteData(
          table: "content_type",
          keyColumn: "id",
          keyValue: 1);

      migrationBuilder.DeleteData(
          table: "content_type",
          keyColumn: "id",
          keyValue: 2);

      migrationBuilder.DeleteData(
          table: "content_type",
          keyColumn: "id",
          keyValue: 3);

      migrationBuilder.DeleteData(
          table: "content_type",
          keyColumn: "id",
          keyValue: 4);

      migrationBuilder.DeleteData(
          table: "content_type",
          keyColumn: "id",
          keyValue: 5);

      migrationBuilder.DeleteData(
          table: "content_type",
          keyColumn: "id",
          keyValue: 6);

      migrationBuilder.DeleteData(
          table: "content_type",
          keyColumn: "id",
          keyValue: 7);

      migrationBuilder.DeleteData(
          table: "content_type",
          keyColumn: "id",
          keyValue: 8);

      migrationBuilder.DeleteData(
          table: "content_type",
          keyColumn: "id",
          keyValue: 9);

      migrationBuilder.DeleteData(
          table: "content_type",
          keyColumn: "id",
          keyValue: 10);

      migrationBuilder.DeleteData(
          table: "content_type",
          keyColumn: "id",
          keyValue: 11);

      migrationBuilder.DeleteData(
          table: "content_type",
          keyColumn: "id",
          keyValue: 12);

      migrationBuilder.DeleteData(
          table: "content_type",
          keyColumn: "id",
          keyValue: 13);

      migrationBuilder.DeleteData(
          table: "content_type",
          keyColumn: "id",
          keyValue: 14);

      migrationBuilder.DeleteData(
          table: "content_type",
          keyColumn: "id",
          keyValue: 15);

      migrationBuilder.DeleteData(
          table: "content_type",
          keyColumn: "id",
          keyValue: 16);

      migrationBuilder.DeleteData(
          table: "content_type",
          keyColumn: "id",
          keyValue: 17);

      migrationBuilder.DeleteData(
          table: "content_type",
          keyColumn: "id",
          keyValue: 18);

      migrationBuilder.DeleteData(
          table: "content_type",
          keyColumn: "id",
          keyValue: 19);

      migrationBuilder.DeleteData(
          table: "content_type",
          keyColumn: "id",
          keyValue: 20);

      migrationBuilder.DeleteData(
          table: "content_type",
          keyColumn: "id",
          keyValue: 21);

      migrationBuilder.DeleteData(
          table: "content_type",
          keyColumn: "id",
          keyValue: 22);

      migrationBuilder.DeleteData(
          table: "content_type",
          keyColumn: "id",
          keyValue: 23);

      migrationBuilder.DeleteData(
          table: "content_type",
          keyColumn: "id",
          keyValue: 24);

      migrationBuilder.DeleteData(
          table: "content_type",
          keyColumn: "id",
          keyValue: 25);

      migrationBuilder.DeleteData(
          table: "content_type",
          keyColumn: "id",
          keyValue: 26);

      migrationBuilder.DeleteData(
          table: "content_type",
          keyColumn: "id",
          keyValue: 27);

      migrationBuilder.DeleteData(
          table: "content_type",
          keyColumn: "id",
          keyValue: 28);

      migrationBuilder.DeleteData(
          table: "content_type",
          keyColumn: "id",
          keyValue: 29);

      migrationBuilder.DeleteData(
          table: "content_type",
          keyColumn: "id",
          keyValue: 30);

      migrationBuilder.DeleteData(
          table: "content_type",
          keyColumn: "id",
          keyValue: 31);

      migrationBuilder.DeleteData(
          table: "content_type",
          keyColumn: "id",
          keyValue: 32);

      migrationBuilder.DeleteData(
          table: "content_type",
          keyColumn: "id",
          keyValue: 33);

      migrationBuilder.DeleteData(
          table: "content_type",
          keyColumn: "id",
          keyValue: 34);

      migrationBuilder.DeleteData(
          table: "content_type",
          keyColumn: "id",
          keyValue: 35);

      migrationBuilder.DeleteData(
          table: "content_type",
          keyColumn: "id",
          keyValue: 36);

      migrationBuilder.DeleteData(
          table: "content_type",
          keyColumn: "id",
          keyValue: 37);

      migrationBuilder.DeleteData(
          table: "content_type",
          keyColumn: "id",
          keyValue: 38);

      migrationBuilder.DeleteData(
          table: "content_type",
          keyColumn: "id",
          keyValue: 39);

      migrationBuilder.DeleteData(
          table: "content_type",
          keyColumn: "id",
          keyValue: 40);

      migrationBuilder.DeleteData(
          table: "content_type",
          keyColumn: "id",
          keyValue: 41);

      migrationBuilder.DeleteData(
          table: "content_type",
          keyColumn: "id",
          keyValue: 42);

      migrationBuilder.DeleteData(
          table: "content_type",
          keyColumn: "id",
          keyValue: 43);

      migrationBuilder.DeleteData(
          table: "content_type",
          keyColumn: "id",
          keyValue: 44);

      migrationBuilder.DeleteData(
          table: "content_type",
          keyColumn: "id",
          keyValue: 45);

      migrationBuilder.DeleteData(
          table: "content_type",
          keyColumn: "id",
          keyValue: 46);

      migrationBuilder.DeleteData(
          table: "content_type",
          keyColumn: "id",
          keyValue: 47);

      migrationBuilder.DeleteData(
          table: "content_type",
          keyColumn: "id",
          keyValue: 48);

      migrationBuilder.DeleteData(
          table: "content_type",
          keyColumn: "id",
          keyValue: 49);

      migrationBuilder.DeleteData(
          table: "content_type",
          keyColumn: "id",
          keyValue: 50);

      migrationBuilder.DeleteData(
          table: "content_type",
          keyColumn: "id",
          keyValue: 51);

      migrationBuilder.DeleteData(
          table: "content_type",
          keyColumn: "id",
          keyValue: 52);

      migrationBuilder.DeleteData(
          table: "content_type",
          keyColumn: "id",
          keyValue: 53);

      migrationBuilder.DeleteData(
          table: "content_type",
          keyColumn: "id",
          keyValue: 54);

      migrationBuilder.DeleteData(
          table: "content_type",
          keyColumn: "id",
          keyValue: 55);

      migrationBuilder.DeleteData(
          table: "content_type",
          keyColumn: "id",
          keyValue: 56);

      migrationBuilder.DeleteData(
          table: "content_type",
          keyColumn: "id",
          keyValue: 57);

      migrationBuilder.DeleteData(
          table: "content_type",
          keyColumn: "id",
          keyValue: 58);

      migrationBuilder.DeleteData(
          table: "content_type",
          keyColumn: "id",
          keyValue: 59);

      migrationBuilder.DeleteData(
          table: "content_type",
          keyColumn: "id",
          keyValue: 60);

      migrationBuilder.DeleteData(
          table: "content_type",
          keyColumn: "id",
          keyValue: 61);

      migrationBuilder.DeleteData(
          table: "content_type",
          keyColumn: "id",
          keyValue: 62);

      migrationBuilder.DeleteData(
          table: "content_type",
          keyColumn: "id",
          keyValue: 63);

      migrationBuilder.DeleteData(
          table: "content_type",
          keyColumn: "id",
          keyValue: 64);

      migrationBuilder.DeleteData(
          table: "content_type",
          keyColumn: "id",
          keyValue: 65);

      migrationBuilder.DeleteData(
          table: "content_type",
          keyColumn: "id",
          keyValue: 66);

      migrationBuilder.DeleteData(
          table: "content_type",
          keyColumn: "id",
          keyValue: 67);

      migrationBuilder.DeleteData(
          table: "content_type",
          keyColumn: "id",
          keyValue: 68);
    }
  }
}
