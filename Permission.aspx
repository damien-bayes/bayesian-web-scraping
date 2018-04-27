<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Permission.aspx.cs" Inherits="Permission" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml"
      lang="en"
      xml:lang="en">

    <head>
        <meta http-equiv="Content-Type" 
              content="text/html; charset=utf-8" />
        
        <meta name="viewport" content="width=device-width, initial-scale=1.0" />
        
        <meta name="language" 
              content="en" />

        <title>Cube Crawler: You are using an outdated browser</title>

        <link rel="shortcut icon" 
              href="/images/cube-crawler-supervisor-images/favicon.ico" />

        <link rel="stylesheet" 
              href="/css/cube-crawler-supervisor-css/cube-crawler-supervisor.css" />

        <style>
            html, body {
                width: 100%;
                height: 100%;
                background: #ffffff;

                background-image: url(/images/background.png);
                background-attachment: fixed; 
                background-repeat: no-repeat; 
                background-position: center; 
                background-size:cover;

                padding: 0px;
                margin: 0px;
            }
            #bad_browser {
                position: absolute;
                left: 50%;
                top: 50%;
                text-align: center;
                width: 530px;
                margin: -200px 0px 0px -250px;
                line-height: 180%;
            }
            #content {
                padding: 20px;
                font-size: 16px;
            }
            #content div {
                margin: 10px 0 16px 0;
            }
            #content #browsers {
                height: 136px;

                margin: 4px 160px 0px;
            }
            #browsers a {
                float: left;
                width: 180px;

                border-radius: 4px;
                -webkit-border-radius: 4px;
                -moz-border-radius: 4px;
            }
            #browsers a:hover {
                text-decoration: none;
                background-color: #E0F2F1 !important;
            }
        </style>
    </head>

    <body>
        <div id="bad_browser" class="shadow-z-1">
            <div id="head"></div>
            <div id="wrap">
                <div id="content">
                    This content is no longer available.
                    <div>
                        The content you requested cannot be displayed. You do not have permission to view this content.
                            <div id="browsers" style="width: 400px;">
                                <a href="/Login.aspx">
                                    <img style="width: 160px;" src="/images/cube-crawler-supervisor-images/svg/cube-logo-3-1.svg"/>Back
                                </a>
                            </div>
                    </div>
                </div>
            </div>
        </div>
    </body>

</html>