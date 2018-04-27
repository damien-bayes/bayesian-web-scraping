<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Test.aspx.cs" Inherits="Test" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
        <meta http-equiv="Content-Type" 
              content="text/html; charset=utf-8"/>
    
        <meta http-equiv="X-UA-Compatible" 
              content="IE=edge" />

        <meta name="viewport" 
              content="width=device-width, initial-scale=1, minimum-scale=1, maximum-scale=1" />

        <meta name="language" 
              content="ru" />

        <meta name="robots" 
              content="index, follow" />

        <meta name="author" 
              content="Damien Bayes"/>

        <meta name="copyright" 
              lang="ru" 
              content="Cube Crawler. Все права защищены."/>

        <title></title>

        <link rel="shortcut icon" 
              href="/images/cube-crawler-supervisor-images/favicon.ico" />

        <link rel="stylesheet" 
              href="/css/cube-crawler-supervisor-css/cube-crawler-supervisor.css" />

        <link rel="stylesheet"
              href="/css/cube-crawler-supervisor-css/pages/master-page.css"/>

        <noscript><meta http-equiv="refresh" content="0; URL=/OutdatedBrowser.aspx"></noscript>

        <script type="text/javascript" 
                src="/js/jquery/jquery-1.10.2.js"></script>

        <script type="text/javascript"
                 src="/js/cube-crawler-supervisor-js/cube-crawler-supervisor-bootstrap.js"></script>

        <script type="text/javascript"
                src="/js/cube-crawler-supervisor-js/cube-crawler-supervisor-ripples.js"></script>

        <script type="text/javascript"
                src="/js/cube-crawler-supervisor-js/cube-crawler-supervisor-material.js"></script>

        <script type="text/javascript"
                src="js/cube-crawler-supervisor-js/extensions/cube-crawler-supervisor-extensions.js"></script>

        <style>
            .jq-dropdown {
                position: relative;
                width: 200px;
                padding: 10px;
                margin: 0 auto;

                background: #fafafa;
                color: black;
                outline: none;
                cursor: pointer;
            }

            .jq-dropdown:after {
                content: "";
                width: 0;
                height: 0;
                position: absolute;
                right: 16px;
                top: 50%;
                margin-top: -6px;
                border-width: 6px 0 6px 6px;
                border-style: solid;
                border-color: transparent #fff;
            }

            .jq-dropdown .jq-dropdown-menu {
                position: absolute;
                top: 100%;
                left: 0;
                right: 0;

                background: #fff;
                font-weight: normal;

                /* opacity: 0; */
                pointer-events: none;
            }

            .jq-dropdown .jq-dropdown-menu li {
                list-style-type: none;
            }

            .jq-dropdown .jq-dropdown-menu li a {
                display: block;
                text-decoration: none;
                color: #9e9e9e;
                padding: 10px 20px;
            }

            .jq-dropdown.jq-dropdown-active .jq-dropdown-menu {
                opacity: 1;
                pointer-events: all;
            }
        </style>
</head>

<body>

    <div id="jq-dropdown-1" class="jq-dropdown jq-dropdown-tip">
        <ul class="jq-dropdown-menu">
            <li>
                I'm hidden!
            </li>
            <li>
                Me too!
            </li>
            <li>
                So do I.
            </li>
        </ul>
    </div>

    <script type="text/javascript">
        (function($, window, document, undefined) {
            
            "use strict";

            /**
             * Define the name of the plugin
             */
            var ripples = "ripples";


            /**
             * Get an instance of the plugin
             */
            var self = null;


            /**
             * Define the defaults of the plugin
             */
            var defaults = {};

            /**
             * Create the main plugin function
             */
            function Ripples(element, options) {
                self = this;

                this.element = $(element);

                this.options = $.extend({}, defaults, options);

                this._defaults = defaults;
                this._name = ripples;

                alert('Hello');

                this.init();
            }

            /**
             * Initialize the plugin
             */
            Ripples.prototype.init = function () {
                var $element = this.element;
                alert('Hello Init');
            }

            /**
             * Create the jquery plugin function
             */
            $.fn.ripples = function (options) {
                return this.each(function () {
                    if (!$.data(this, "plugin_" + ripples)) {
                        $.data(this, "plugin_" + ripples, new Ripples(this, options));
                    }
                });
            };
        })(jQuery, window, document);

        $(document).ready(function () {
            this.$.ripples();
        });
    </script>

</body>

</html>
