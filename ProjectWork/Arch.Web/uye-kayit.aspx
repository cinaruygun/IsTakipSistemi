<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="uye-kayit.aspx.cs" Inherits="Arch.Web.uye_kayit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Üye Ol</title>
    <link href="Assets/plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="Assets/css/ercanayhan.min.css" rel="stylesheet" />
    <link href="Assets/css/kendo.common-material.min.css" rel="stylesheet" />
    <link href="Assets/css/kendo.material.min.css" rel="stylesheet" />
    <link href="Assets/css/kendo.material.mobile.min.css" rel="stylesheet" />
    <link href="Assets/css/materialize.css" rel="stylesheet" />
    
</head>
<body class="login-page ls-closed">
    <form id="form1" runat="server">
        <div class="login-box">
             <div class="logo">
        <a href="javascript:void(0);">İş Takip Sistemi</a>
        <small>Çınar Uygun-Fatih Bilgin</small>
    </div>
        </div>

         <div class="card">
        <div class="body mask">
            <div >
                <div class="msg">Üye Kayıt Olurken Boş Alan Bırakmayınız
                    <asp:Label Text="" ID="hatatxt" runat="server" />
                </div>
                <div class="input-group">
                    <span class="input-group-addon">
                        <i class="material-icons">person</i>
                    </span>
                    <div class="form-line">
                        <input type="text" id="adi" runat="server" class="form-control" name="username" data-attribute="UserName" placeholder="Adı" required="required" autofocus="autofocus" />
                    </div>
                </div>
                <div class="input-group">
                    <span class="input-group-addon">
                        <i class="material-icons">person</i>
                    </span>
                    <div class="form-line">
                        <input type="text" id="soyadi" runat="server" class="form-control" name="username" data-attribute="UserName" placeholder="Soyadı" required="required" autofocus="autofocus" />
                    </div>
                </div>
                   <div class="input-group">
                    <span class="input-group-addon">
                        <i class="material-icons">person</i>
                    </span>
                    <div class="form-line">
                        <input type="text" id="kullaniciAdi" runat="server" class="form-control" name="username" data-attribute="UserName" placeholder="Kullanıcı Adı" required="required" autofocus="autofocus" />
                    </div>
                </div>
                
                 <div class="input-group">
                    <span class="input-group-addon">
                        <i class="material-icons">email</i>
                    </span>
                    <div class="form-line">
                        <input type="email" id="email" runat="server" class="form-control" name="username" data-attribute="UserName" placeholder="Mail Adresi" required="required" autofocus="autofocus" />
                    </div>
                </div>
                <div class="input-group">
                    <span class="input-group-addon">
                        <i class="material-icons">edit</i>
                    </span>
                    <asp:DropDownList runat="server" ID="unitlist" CssClass="form-control">
                        <asp:ListItem Text="Birimi Seçiniz" Selected="True" />
                    </asp:DropDownList>
                </div>
                <div class="input-group">
                    <span class="input-group-addon">
                        <i class="material-icons">lock</i>
                    </span>
                    <div class="form-line">
                        <input type="password"  id="sifre" runat="server"  class="form-control" name="password" data-attribute="Password" placeholder="Şifreniz" required="required" autofocus="autofocus" />
                    </div>
                </div>
                <div class="row">
                  
                
                    <div class="col-xs-4">
                        
                         <asp:Button OnClick="Kaydetbtn_Click" ID="Kaydetbtn" Text="Kayıt Ol" class="btn btn-block bg-pink waves-effect"  runat="server" />
                    </div>
                    <div class="col-xs-4">
                        <a href="/Login/Index" class="btn btn-block bg-pink waves-effect">Giriş Yap</a>
                    </div>
                </div>
            </div>
        </div>
    </div>
        <script src="Assets/plugins/jquery/jquery.js"></script>
        <script src="Assets/plugins/bootstrap/js/bootstrap.min.js"></script>
 
    </form>
</body>
</html>
