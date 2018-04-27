<%@ Page Language="C#" AutoEventWireup="true" CodeFile="hi_result.aspx.cs" Inherits="SHost_hi_result" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Результат вычислений</title>
    <link href="../css/metro-bootstrap.css" rel="stylesheet" />
    <link href="../css/metro-bootstrap-responsive.css" rel="stylesheet" />
    <link href="../css/iconFont.css" rel="stylesheet" />
    <link href="../css/docs.css" rel="stylesheet" />
    <link href="../css/core.css" rel="stylesheet" />
    <script type="text/javascript" src="../js/jquery-1.10.2.js"></script>
    <script type="text/javascript" src="../js/jquery.widget.js"></script>
    <script type="text/javascript" src="../js/metro.min.js"></script>
</head>
<body class="metro">
    <form id="form1" runat="server">
    <div class="container">
        <br />
        <div style="position:relative; height: 110px;">
            <h1 class="slider_" style="position: absolute; top: 0px; left: 30px; opacity: 0;">
                Результат<small class="on-right">вычисления</small>
            </h1>
            <p class="description slider_" style="position: absolute; top: 90px; left: 20px; opacity: 0;">
                Страница вывода результата ChI-квадрата.
            </p>
        </div>
        <hr />
         <div class="grid">
             <table class="table striped hovered dataTable" id="dataTables-1" aria-describedby="dataTables-1_info">
                <thead>
                    <tr role="row">
                        <th class="text-left sorting_asc" tabindex="0" aria-controls="dataTables-1" rowspan="1" colspan="1" aria-label="Engine: activate to sort column ascending" style="width: 162px;" aria-sort="ascending">Слово</th>
                        <th class="text-left sorting" tabindex="0" aria-controls="dataTables-1" rowspan="1" colspan="1" aria-label="Browser: activate to sort column ascending" style="width: 286px;">А</th>
                        <th class="text-left sorting" tabindex="0" aria-controls="dataTables-1" rowspan="1" colspan="1" aria-label="Platform: activate to sort column ascending" style="width: 267px;">B</th>
                        <th class="text-left sorting" tabindex="0" aria-controls="dataTables-1" rowspan="1" colspan="1" aria-label="Platform: activate to sort column ascending" style="width: 267px;">C</th>
                        <th class="text-left sorting" tabindex="0" aria-controls="dataTables-1" rowspan="1" colspan="1" aria-label="Platform: activate to sort column ascending" style="width: 267px;">D</th>
                        <th class="text-left sorting" tabindex="0" aria-controls="dataTables-1" rowspan="1" colspan="1" aria-label="Platform: activate to sort column ascending" style="width: 267px;">ChI</th>
                        <th class="text-left sorting" tabindex="0" aria-controls="dataTables-1" rowspan="1" colspan="1" aria-label="Platform: activate to sort column ascending" style="width: 267px;">MI</th>
                        <th class="text-left sorting" tabindex="0" aria-controls="dataTables-1" rowspan="1" colspan="1" aria-label="Platform: activate to sort column ascending" style="width: 267px;">IG</th>
                    </tr>
                </thead>

                <tbody id="his" runat="server">
                            
                </tbody>
                <tfoot>
                    <tr>
                        <th class="text-left sorting_asc" tabindex="0" aria-controls="dataTables-1" rowspan="1" colspan="1" aria-label="Engine: activate to sort column ascending" style="width: 162px;" aria-sort="ascending">Слово</th>
                        <th class="text-left sorting" tabindex="0" aria-controls="dataTables-1" rowspan="1" colspan="1" aria-label="Browser: activate to sort column ascending" style="width: 286px;">А</th>
                        <th class="text-left sorting" tabindex="0" aria-controls="dataTables-1" rowspan="1" colspan="1" aria-label="Platform: activate to sort column ascending" style="width: 267px;">B</th>
                        <th class="text-left sorting" tabindex="0" aria-controls="dataTables-1" rowspan="1" colspan="1" aria-label="Platform: activate to sort column ascending" style="width: 267px;">C</th>
                        <th class="text-left sorting" tabindex="0" aria-controls="dataTables-1" rowspan="1" colspan="1" aria-label="Platform: activate to sort column ascending" style="width: 267px;">D</th>
                        <th class="text-left sorting" tabindex="0" aria-controls="dataTables-1" rowspan="1" colspan="1" aria-label="Platform: activate to sort column ascending" style="width: 267px;">ChI</th>
                        <th class="text-left sorting" tabindex="0" aria-controls="dataTables-1" rowspan="1" colspan="1" aria-label="Platform: activate to sort column ascending" style="width: 267px;">MI</th>
                        <th class="text-left sorting" tabindex="0" aria-controls="dataTables-1" rowspan="1" colspan="1" aria-label="Platform: activate to sort column ascending" style="width: 267px;">IG</th>
                    </tr >
                </tfoot>
            </table>
        </div>
    </div>
    </form>
</body>
</html>
<script type="text/javascript" src="../js/core.js" ></script>
<script type ="text/javascript">
    Win8_effectHeader();

</script>
