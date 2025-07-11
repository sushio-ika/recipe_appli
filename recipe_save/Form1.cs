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
    public partial class title : Form
    {
        public static int windowsize = 0;

        private static string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=recipeDB;Integrated Security=True;";

        public class RecipeDisplayModel
        {
            public int RecipeID { get; set; }
            public string RecipeName { get; set; }
        }

        public static void Load_RecipeList(DataGridView dataGridView1)
        {
            List<RecipeDisplayModel> recipes = new List<RecipeDisplayModel>(); // データを格納するリスト

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    Console.WriteLine("データベースに接続しました。レシピ情報を取得します..."); // デバッグ用

                    string sql = "SELECT recipeID, recipeName FROM [dbo].[RECIPE.Table] ORDER BY recipeID ASC"; // 順序を追加

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        // ここで command.Parameters.AddWithValue() は不要（SQLがパラメーターを使っていないため）
                        // 以前のコードでここにあった行は削除

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // データを読み込み、モデルオブジェクトに格納し、リストに追加
                                RecipeDisplayModel recipe = new RecipeDisplayModel
                                {
                                    RecipeID = reader.GetInt32(reader.GetOrdinal("recipeID")),
                                    RecipeName = reader.GetString(reader.GetOrdinal("recipeName"))
                                };
                                recipes.Add(recipe);
                            }
                        }
                    }

                    // DataGridView にデータをバインド
                    dataGridView1.DataSource = recipes;

                    // DataGridView の列設定はデータバインド後、かつループ外で一度だけ行う
                    if (dataGridView1.Columns.Contains("RecipeID"))
                    {
                        dataGridView1.Columns["RecipeID"].HeaderText = "レシピID";
                        dataGridView1.Columns["RecipeID"].Width = 80; // 幅を固定するなど
                    }
                    if (dataGridView1.Columns.Contains("RecipeName"))
                    {
                        dataGridView1.Columns["RecipeName"].HeaderText = "レシピ名";
                        dataGridView1.Columns["RecipeName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show($"データベースエラーが発生しました: {ex.Message}", "データベースエラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Console.WriteLine($"データベースエラー: {ex.Message}"); // デバッグ用
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"予期せぬエラーが発生しました: {ex.Message}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Console.WriteLine($"予期せぬエラー: {ex.Message}"); // デバッグ用
                }
            }
        }

        public void ChangeWindowSize(int size)
        {
            if (size == 0)
            {
                this.Size = new Size(1000, 600);
            }
            else
            {
                Screen currentScreen = Screen.FromControl(this);
                Rectangle workingArea = currentScreen.WorkingArea;

                this.Location = workingArea.Location;
                this.Size = workingArea.Size;
            }
            title_Load(null, null);
        }
        public title()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            ChangeWindowSize(windowsize);
        }

        private void title_Load(object sender, EventArgs e)
        {
            int width_size = this.Width;
            int height_size = this.Height;

            button1.Location = new Point(width_size / 2 - button1.Width / 2 - 400, height_size / 2 - button1.Height / 2 + 200);
            dataGridView1.Location = new Point(width_size / 2 - dataGridView1.Width / 2, height_size / 2 - dataGridView1.Height / 2 + 50);
            Load_RecipeList(dataGridView1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            touroku form = new touroku();
            form.ShowDialog();
            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                windowsize = 0;
            }
            else
            {
                windowsize = 1;
            }
            ChangeWindowSize(windowsize);
        }
    }
}
