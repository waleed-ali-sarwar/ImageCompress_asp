<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="image_page.aspx.cs" Inherits="ImageCompress_asp.image_page" %>

<!DOCTYPE html>
<html>

<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="initial-scale=1.0, width=device-width, height=device-height" />
    <title>Image Compressor + Resizer + Preview</title>
    <style>
        .preview-container {
            margin-top: 15px;
            display: flex;
            flex-wrap: wrap;
            gap: 10px;
        }

            .preview-container img {
                max-width: 200px;
                max-height: 200px;
                border: 1px solid #ccc;
                padding: 3px;
                border-radius: 4px;
                object-fit: contain;
            }
    </style>
    <script type="text/javascript">
        function previewImages(input) {
            var preview = document.getElementById('preview');
            preview.innerHTML = "";

            if (input.files) {
                Array.from(input.files).forEach(file => {
                    if (file.type.startsWith("image/")) {
                        var reader = new FileReader();
                        reader.onload = function (e) {
                            var img = document.createElement("img");
                            img.src = e.target.result;
                            preview.appendChild(img);
                        };
                        reader.readAsDataURL(file);
                    }
                });
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <h2>Upload Images to Compress & Download</h2>
        <asp:FileUpload ID="FileUpload1" runat="server" AllowMultiple="true" onchange="previewImages(this)" />
        <br />
        <br />
        Max Width:
        <asp:TextBox ID="TxtMaxWidth" runat="server" Text="0" />
        Max Height:
        <asp:TextBox ID="TxtMaxHeight" runat="server" Text="0" />
        <span>* optional</span>
        <br />
        <br />
        <asp:Button ID="BtnUpload" runat="server" Text="Upload & Process" OnClick="BtnUpload_Click" />
        <br />
        <br />
        <div id="preview" class="preview-container"></div>
        <br />
        <asp:Literal ID="LitResult" runat="server" />
    </form>
    <footer>
        for any issue <a href="https://linkedin.com/in/waleed-ali-sarwar/">contact to administrator</a>
    </footer>
</body>
</html>
