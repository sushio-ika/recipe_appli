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
using static System.Windows.Forms.AxHost;

namespace recipe_save
{
    public partial class touroku : Form
    {
        public touroku()
        {
            InitializeComponent();
            if (title.windowsize == 0)
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
        }
        private int textBoxCount = 0;
        private int startY = 200;
        // 各テキストボックス間の間隔
        private const int VERTICAL_SPACING = 40;
        private string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=recipeDB;Integrated Security=True;";

        public bool AddRecipe(string recipeName)
        {
            // SQLクエリ: RECIPE.Table に recipeID と recipeName を挿入
            // パラメーター (@recipeID, @recipeName) を使用してSQLインジェクションを防ぐ
            string sql = "INSERT INTO [dbo].[RECIPE.Table] (recipeName) VALUES (@recipeName); SELECT SCOPE_IDENTITY(); ";

            // SqlConnection オブジェクトを using ブロックで作成し、確実にリソースを解放
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // SqlCommand オブジェクトを using ブロックで作成
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    // パラメーターを追加
                    // AddWithValue は型を自動判別してくれるため便利
                    command.Parameters.AddWithValue("@recipeName", recipeName);

                    try
                    {
                        connection.Open();
                        // ExecuteScalar() で自動生成されたID（decimal型）を取得し、intに変換
                        // もしSCOPE_IDENTITY()がNULLを返す可能性がある場合（例: IDENTITY_INSERTがONの時）、Convert.ToInt32はエラーになるので注意
                        int newRecipeId = Convert.ToInt32(command.ExecuteScalar());
                        Console.WriteLine($"ADO.NET: 新しいレシピ '{recipeName}' が追加されました。ID: {newRecipeId}");
                        return true; // 成功した場合は true を返す
                    }
                    catch (SqlException ex)
                    {
                        // SQL Server固有のエラーをキャッチ
                        Console.WriteLine($"ADO.NET エラーが発生しました: {ex.Message}");
                        // 例: 主キー重複 (recipeID が既に存在する場合) など
                        return false;
                    }
                    catch (Exception ex)
                    {
                        // その他の予期せぬエラーをキャッチ
                        Console.WriteLine($"予期せぬエラーが発生しました: {ex.Message}");
                        return false;
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string recipeName = textBox1.Text; // テキストボックスからレシピ名を取得
            string recipeTime = textBox3.Text; // テキストボックスからレシピ時間を取得
            if (string.IsNullOrWhiteSpace(recipeName))
            {
                MessageBox.Show("レシピ名を入力してください。");
                return;
            }
            // touroku フォーム自身の AddRecipe メソッドを呼び出す
            bool success = AddRecipe(recipeName); // AddRecipe メソッドは bool を返す

            if (success)
            {
                MessageBox.Show("レシピが正常に保存されました！");
                this.Close(); // 成功したらフォームを閉じる（任意）
            }
            else
            {
                MessageBox.Show("レシピの保存に失敗しました。詳細をコンソールで確認してください。");
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            string path; //パス名(ファイル名)
                         //オープンファイルダイアログでファイル選択し「OK」が押されたら
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                path = openFileDialog1.FileName; //パスを取得しpathに格納

            }
            else
            {
                return; //キャンセルが押されたらここで処理終了

            }

            pictureBox1.ImageLocation = path; //選択した画像のパスをpictureBox1に設定
        }

        /*private void button2_Click(object sender, EventArgs e)
        {
            TextBox newTextBox = new TextBox();

            // 2. プロパティを設定
            newTextBox.Name = "textBoxDynamic_" + textBoxCount; // ユニークな名前を付ける
            newTextBox.Location = new Point(
                this.textBox2.Left, // ボタンと同じX座標に配置
                startY + (textBoxCount * VERTICAL_SPACING) // 前のテキストボックスの下に配置
            );
            newTextBox.Font = textBox2.Font; // 既存のテキストボックスと同じフォントにする
            newTextBox.Size = textBox2.Size; // 既存のテキストボックスと同じサイズにする
            newTextBox.Anchor = AnchorStyles.Left | AnchorStyles.Right; // フォームのリサイズに追従させる（任意）

            // 4. フォームに新しいテキストボックスを追加
            this.Controls.Add(newTextBox);

            // 5. カウントを増やす
            textBoxCount++;

            // フォームの高さが足りなくなる場合に、自動的にフォームを拡張する（任意）
            if (newTextBox.Bottom + 10 > this.ClientSize.Height)
            {
                this.ClientSize = new Size(this.ClientSize.Width, newTextBox.Bottom + 10);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //最近追加したテキストボックスを削除する

        }*/
    }
}
