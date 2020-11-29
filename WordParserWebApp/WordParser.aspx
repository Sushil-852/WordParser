<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WordParser.aspx.cs" Inherits="WordParserWebApp.WordParser" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Word Parse</title>
    
</head>

<body>
    <form id="form1" runat="server">
   <div>
            <div>
                <label style="font-size:medium;font-weight:700">Word Parser using WordParser API</label>
                <br />
                <br />
            </div>
            <div>
                 <label style="font-style:italic">This parser will parse the frequency of occurence of words in text file</label><br />
                <br />
               <label>Enter FileURL </label>
               <input type="text" id="fileurl" style="width:300px;" name="fileurl"/>&nbsp;<input type="button" name="parsefile" id="parsefile" value="Parse File"/>
               <br />
            </div>
            <div id="top10" ></div>
            <div id="searchword" style="display:none">
                 <input type="text" id="searchwrd" style="width:200px;" name="searchwrd"/>&nbsp;<input type="button" name="searchbtn" id="searchbtn" value="Search Word"/>
                
            </div>
           <div id="searchresult" ></div>
        </div>
    </form>
    <script src="https://code.jquery.com/jquery-3.5.1.min.js" ></script>
    <script >
        $(document).ready(function () {
            $("#parsefile").click(function () {
                ParseFile();
            });
            $("#searchbtn").click(function () {
                SearchWord();
            });
        });
        function ParseFile() {

            var fileURL = $("#fileurl").val();

            $.ajax({
                type: "POST",
                url: "http://localhost/WordParserAPI/ParseFile?fileURL=" + fileURL,
                dataType: 'json',
                async: false,
                data: { 'fileURL': fileURL },
                success: function (response) {

                    var data = response;
                    console.log(data[0].Key);
                    var html = '<table><thead><tr><th>Word</th><th>Count</th></tr></thead><tbody>';
                    for (var i = 0; i < data.length; i++) {
                        console.log(data[i].Key);
                        html += '<tr>';

                        html += '<td>' + data[i].Key + '</td>';
                        html += '<td>' + data[i].Value + '</td>';

                        html += "</tr>";
                    }
                    html += '</tbody></table>';

                    $(html).appendTo('#top10');
                    $("#searchword").show();
                },
                failure: function (response) {

                    console.log(response);

                },
                error: function (err) {
                    console.log(err);
                }
            });
        }
        function SearchWord() {

            var srchWord = $("#searchwrd").val();
            alert(srchWord);
            $.ajax({
                type: "GET",
                url: "http://localhost/WordParserAPI/Searchword?searchWord=" + srchWord,
                dataType: 'json',
                async: false,

                success: function (response) {

                    var data = JSON.stringify(response, undefined, 4);
                    console.log(data)
                    var html = '<br><textarea id="searchresult" style="width:400px;height:400px">' + data + '</textarea>';
                    console.log(html)
                    $(html).appendTo('#searchresult');
                },
                failure: function (response) {

                    console.log(response);

                },
                error: function (err) {
                    console.log(err);
                }
            });
        }

    </script>
</body>
</html>
