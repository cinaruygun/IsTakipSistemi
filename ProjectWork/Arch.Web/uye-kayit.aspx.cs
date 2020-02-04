using Arch.Core.Constants;
using Arch.Utilities.Manager;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Arch.Web
{
    public partial class uye_kayit : System.Web.UI.Page
    {
        SqlConnection baglanti = new SqlConnection();
        string adres = ConfigurationManager.ConnectionStrings["ArchEntities"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                baglanti.ConnectionString = adres;
                baglanti.Open();
                SqlCommand cmd = new SqlCommand("select Id, Name from common.Unit", baglanti);
                SqlDataReader oku = cmd.ExecuteReader();
                while (oku.Read())
                {
                    unitlist.Items.Add(new ListItem(oku["Name"].ToString(), oku["Id"].ToString()));
                }
                oku.Close();
                baglanti.Close();
            }



        }
       
        protected void Kaydetbtn_Click(object sender, EventArgs e)
        {
           

            baglanti.ConnectionString = adres;
            baglanti.Open();

            string sql = "insert into common.Person values (@UnitId,@UserName,@kisa, @Name,@Suname,@Password, @Email, 2332,@booll, null)";
            SqlCommand cmd = new SqlCommand(sql, baglanti);
            cmd.Parameters.AddWithValue("@Name", adi.Value);
            cmd.Parameters.AddWithValue("@Suname", soyadi.Value);
            cmd.Parameters.AddWithValue("@UserName", kullaniciAdi.Value);
            cmd.Parameters.AddWithValue("@Email", email.Value);
            cmd.Parameters.AddWithValue("@UnitId", unitlist.SelectedItem.Value);
            cmd.Parameters.AddWithValue("@Password", EnDeCode.Encrypt(sifre.Value, StaticParams.SifrelemeParametresi));
            cmd.Parameters.AddWithValue("@booll", "False");
            cmd.Parameters.AddWithValue("@kisa", adi.Value.Substring(0, 1).ToUpper() + soyadi.Value.Substring(0, 1).ToUpper());
            cmd.ExecuteNonQuery();
            baglanti.Close();
            Response.Redirect("/Login/Index");



        }
    }
}