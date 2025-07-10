using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace recipe_save
{
    public partial class Form1 : Form
    {
        private static string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=recipeDB;Integrated Security=True;";

        public static void Save_Recipe()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    Console.WriteLine("データベースに接続しました。ユーザー情報を取得します...");

                    // 実行したいSQLクエリ
                    string sql = "SELECT recipeID, recipeName FROM [dbo].[RECIPE.Table]";

                    // SqlCommand オブジェクトを作成
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        // SqlDataReader を使用して結果を読み込む
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            command.Parameters.AddWithValue("@recipeID", 1); // 例としてパラメータを追加
                            command.Parameters.AddWithValue("@recipeName", "サンプルレシピ"); // 例としてパラメータを追加

                            Console.WriteLine("\n--- レシピ一覧 ---");
                            // reader.Read() は、次の行が読み込めたら true を返す
                            while (reader.Read())
                            {
                                // データの取得方法：
                                // 1. 列のインデックスで取得 (例: reader.GetInt32(0))
                                // 2. 列名で取得 (reader.GetOrdinal("列名") でインデックスを取得してから)
                                int id = reader.GetInt32(reader.GetOrdinal("recipeID"));
                                string name = reader.GetString(reader.GetOrdinal("recipeName"));

                                Console.WriteLine($"ID: {id}, 料理名: {name}");
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine($"データベースエラーが発生しました: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"予期せぬエラーが発生しました: {ex.Message}");
                }
            }
        }
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int width_size=this.Width;
            int height_size=this.Height;

            button1.Location = new Point(width_size / 2 - button1.Width / 2, height_size / 2 - button1.Height / 2);
            Save_Recipe();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            touroku form = new touroku();
            form.ShowDialog();
            this.Close();
        }
    }
}
