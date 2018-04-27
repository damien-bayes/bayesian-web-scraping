<%@ Page Language="C#" 
         AutoEventWireup="true" 
         CodeFile="Login.aspx.cs" 
         Inherits="Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml"
      lang="ru"
      xml:lang="ru-ru">

<head>

    <meta http-equiv="Content-Type" 
          content="text/html; charset=utf-8"/>
    
    <meta http-equiv="X-UA-Compatible" 
          content="IE=edge" />

    <meta name="viewport" 
          content="width=device-width, initial-scale=1, minimum-scale=1, maximum-scale=1" />

    <meta name="language" 
          content="ru" />

    <meta name="description" 
          content="" />

    <meta name="keywords" 
          content="" />

    <meta name="robots" 
          content="index, follow" />

    <meta name="author" 
          content="Damien Bayes"/>

    <meta name="copyright" 
          lang="ru" 
          content="Cube Crawler. Все права защищены."/>

    <title>Cube Crawler: Авторизация пользователя</title>

    <link rel="shortcut icon" 
          href="/images/cube-crawler-supervisor-images/favicon.ico" />

    <link rel="stylesheet" 
          href="/css/cube-crawler-supervisor-css/cube-crawler-supervisor.css" />

    <link rel="stylesheet"
          href="/css/cube-crawler-supervisor-css/pages/login-page.css"/>

    <noscript><meta http-equiv="refresh" content="0; URL=/OutdatedBrowser.aspx"></noscript>

    <script type="text/javascript" 
            src="/js/jquery/jquery-1.10.2.js"></script>

    <script type="text/javascript"
            src="/js/cube-crawler-supervisor-js/cube-crawler-supervisor-ripples.js"></script>

    <script type="text/javascript"
            src="/js/cube-crawler-supervisor-js/cube-crawler-supervisor-material.js"></script>
</head>

<body>

    <div class="grid-container">
        <div class="grid-100 grid-parent">
            <div class="grid-100">
                <img class="logo" 
                     title="Cube Crawler: Авторизация пользователя" 
                     src="/images/cube-crawler-supervisor-images/svg/cube-logo-2.svg"/>
            </div>

            <div class="grid-100">
                <h1 class="text-center" 
                    title="Предоставление определенному лицу или группе лиц прав на выполнение определенных действий, а также процесс проверки (подтверждения) данных прав при попытке выполнения этих действий.">
                    <span class="fa fa-sign-in"></span>&nbsp;
                    Авторизация пользователя
                </h1>
            </div>

            <div class="grid-50 push-25 grid-parent shadow-z-1 input-indent">
                <form id="form1" 
                      runat="server">
                    <div class="grid-50 top-indent">
                        <p class="text-center" 
                           title="Имя пользователя необходимо для осуществления действий, связанных с Cube Crawler.">
                            <span class="fa fa-user"></span>&nbsp;
                            Имя пользователя:
                        </p>
                    </div>
                    <div class="grid-50 input-indent">
                        <input type ="text" 
                               runat ="server" 
                               id="username"
                               class="form-control floating-label" 
                               required="required" 
                               placeholder="Введите Ваш логин" 
                               tabindex="1"/>
                    </div>
                    <div class="grid-50 label-indent">
                        <p class="text-center" 
                           title="Пароль предназначен для подтверждения личности или полномочий.">
                            Пароль:
                        </p>
                    </div>
                    <div class="grid-50 input-indent">
                        <input type ="password" 
                               runat ="server" 
                               id="password" 
                               class="form-control floating-label" 
                               required="required" 
                               placeholder="Введите Ваш пароль"
                               tabindex="2" />
                    </div>
                    <div class="grid-100 input-indent">
                        <asp:Button runat="server" 
                                    CssClass="btn btn-primary form-control loginBtn" 
                                    TabIndex="3" 
                                    OnClick="logIn_Click" 
                                    Text="Авторизация"/>
                    </div>
                    <div class="grid-100 input-indent">
                        <div class="checkbox checkbox-primary">
                            <label>
                                <input type="checkbox" 
                                       runat="server" 
                                       id="chkPersistCookie"/>
                                Оставаться в сети
                            </label>
                        </div>
                    </div>
                    <div class="grid-100 input-indent">
                        <a href="javascript:void(0)">Вы не можете войти?</a>
                    </div>
                    <!--<div class="grid-100 input-indent">
                        <script src="//ulogin.ru/js/ulogin.js"></script>
                        <div id="uLogin" style="text-align: center;" data-ulogin="display=panel;fields=first_name,last_name;providers=vkontakte,mailru,googleplus,google,yandex,liveid,facebook;hidden=;redirect_uri=http%3A%2F%2F"></div>
                    </div>-->
                </form>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        $(document).ready(function () {
            $.material.init();
            $("input").eq(-3).focus();
        });
    </script>
</body>

</html>