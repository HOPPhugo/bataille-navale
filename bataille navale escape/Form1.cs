/*
Nom du projet : Bataille Navale -Try To Escape!-
Auteur : Hugo Schweizer
Date de création : Mardi 8 Avril 2025
Dernière mise à jour : Jeudi 1 Mai 2025
Langage : C# Windows Form.NET Framework 4.7.2
Description : On apprait dans un monde encore inconnu, nous serons affronter à pusieur énigmes, toutes différentes les unes que les autes ! Arriverez-vous à vous en sortir ?
*/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bataille_navale_escape
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
            this.GetType().InvokeMember("DoubleBuffered",
            System.Reflection.BindingFlags.NonPublic |
            System.Reflection.BindingFlags.Instance |
            System.Reflection.BindingFlags.SetProperty,
            null, this, new object[] { true });
        }
        int NBRTouch = 0; // nombre de cases de bateaux touché (une case ne peut être touchée qu'une seule fois)
        int NBRcoulé = 0; // nombre de bateaux coulés ( un bateaux de peut être coulé qu'une seule fois)
        int NBRtir = 0; // quand on clique sur une case blanche le nombre de tir augmente (si la case cliquée n'est pas blanche ( donc déjà touché), le tir ne compte pas
        int Avionscase = 5; //nombre de case qu'un porte-avions doit avoir (à chaque fois que je crée une case de portes-avions, avionscase -1)
        int Torpilleurcase = 2; // nombre de case qu'un Torpilleur doit avoir (à chaque fois que je crée une case de Torpilleur, Torpilleurcase -1)
        bool win = false; // False : n'a pas encore gagner la partie / True : À gagner la partie
        int Contrecase = 3; // nombre de case que le premier Contre-Torpille doit avoir (à chaque fois que je crée une case de Contre-Torpille, Contrecase -1)
        int Contrecase1 = 3;// nombre de case que le deuxième Contre-Torpille doit avoir (à chaque fois que je crée une case de Contre-Torpille, Contrecase -1)
        int croiseurcase = 4; // nombre de case qu'un Croiseur doit avoir (à chaque fois que je crée une case de Croiseur, Croiseurcase -1)
        bool bt1 = false; // false : le bateau 1 n'a pas encore été totalement généré / true : le bateau 1 à été totalement généré
        bool bt2 = false; // false : le bateau 2 n'a pas encore été totalement généré / true : le bateau 2 à été totalement généré
        bool aide = false; // false : Le point d'aide n'a pas encore été utiliser / true : le point d'aide à été utiliser
        bool bt3 = false; // false : le bateau 3 n'a pas encore été totalement généré / true : le bateau 3 à été totalement généré
        bool bt4 = false; // false : le bateau 4 n'a pas encore été totalement généré / true : le bateau 4 à été totalement généré
        bool bt5 = false; // false : le bateau 5 n'a pas encore été totalement généré / true : le bateau 5 à été totalement généré
        Random rpd = new Random();
        int rod;
        string path = AppDomain.CurrentDomain.BaseDirectory; // chemin d'accès de ce fichier

        int bateaux = 0; // le nombre de bateaux généré. (si il est égale à 5, l'écran de chargement disparait)
        List<Button> boates = new List<Button>(); // liste contenant tout les bateaux
        List<Button> ContreList = new List<Button>(); // liste contenant tout les contre1
        List<Button> ContreListe2 = new List<Button>(); // liste contenant tout les contre2
        List<Button> AvionList = new List<Button>(); // liste contenant tout les portes-avions
        List<Button> TorpiListe = new List<Button>(); // liste contenant tout les Torpilleurs
        List<Button> CroiseurList = new List<Button>(); // liste contenant tout les Croiseur

        private void Form1_Load(object sender, EventArgs e)
        {
            Button_grid(); // appelle la fonction qui permet de crée la grille de bouton
            CreateContreBoats(); // appelle la fonction qui permet de généré les Contre-Torpille1
            CreateTorpilleurBoats(); // appelle la fonction qui permet de généré les Torpilleurs
            CreateAvionBoats(); // appelle la fonction qui permet de généré les Portes-avions
            CreateCroiseurBoats(); // appelle la fonction qui permet de généré les Croiseurs
            CreateContre1Boats(); // appelle la fonction qui permet de généré les Contre-Torpille2
        }

        private void Button_grid() // crée la grille de boutosn (10/10)
        {
            var rowCount = 10; // nombre de rangées du tableau
            var columnCount = 10; // nombre de colonnes du tableau

            this.tableLayoutPanel1.ColumnCount = columnCount;
            this.tableLayoutPanel1.RowCount = rowCount;

            this.tableLayoutPanel1.ColumnStyles.Clear();
            this.tableLayoutPanel1.RowStyles.Clear();

            for (int i = 0; i < columnCount; i++) // tant que "i" est plus petit que le nombre de colonnes, i s'incrémente
            {
                this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100 / columnCount)); // ajoute une colonne au tableau
            }
            for (int i = 0; i < rowCount; i++)  // tant que "i" est plus petit que le nombre de rangées, i s'incrémente
            {
                this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100 / rowCount)); // ajoute une rangée au tableau
            }

            for (int i = 0; i < rowCount; i++)  // tant que "i" est plus petit que le nombre de rangées, i s'incrémente
            {
                for (int j = 0; j < columnCount; j++)  // tant que "j" est plus petit que le nombre de colonnes, j s'incrémente
                {

                    var button = new Button();
                    button.BackColor = Color.White;
                    button.Name = string.Format("button_{0}{1}", i, j);
                    button.ForeColor = Color.White;
                    button.Dock = DockStyle.Fill;
                    this.tableLayoutPanel1.Controls.Add(button, j, i); // ajoute le bouton avec son tag : "button_(sa rangée),(sa colonne)
                    button.Click += (s, e) => // lorsqu'on clique sur n'importe quel bouton du tableau
                    {

                        if (button.ForeColor == Color.White && button.BackColor == Color.White) // si je clique sur un bouton blanc ( donc pas enore tiré dessus) j'ajoute 1 à mon nombre de tirs
                        {
                            NBRtir = NBRtir + 1;
                            button.BackColor = Color.LightBlue;
                            label3.Text = "Nombre de tirs : " + NBRtir;
                        }

                        
                    };

                }
            }

        }

        private async void CreateContreBoats() // fonctions générant les Contres-Torpille
        {

            Random rand = new Random();

            int boatsPlaced = 0; // nombre de Contres-Torpille placé

            while (boatsPlaced < 1) // tant que boatsPlaced est plus petit que 1, il continu à se généré
            {

                await Task.Delay(1);

                int x = rand.Next(0, 10); // chiffre aléatoir en X
                int y = rand.Next(0, 10); // chiffre aléatoir en Y
                bool horizontal = rand.Next(0, 2) == 0;

                if (horizontal && x + 1 < 10 && IsPlacementValid(x, y, 3, true)) // si l'endroit où on veut placer le bateau est libre, il se fait
                {
                    for (int i = 0; i < 3; i++) // tant que toutes les cases de son pas faites (3 cases)
                    {
                        var btn = tableLayoutPanel1.GetControlFromPosition(x + i, y) as Button;
                        if (btn != null) // si btn exist ( qu'il n'est pas égale à RIEN )
                        {
                            btn.Tag = "Contre";
                            btn.ForeColor = Color.Black;
                            boates.Add(btn);
                            ContreList.Add(btn);
                            btn.Click += (s, e) =>
                            {
                                if (btn.BackColor == Color.White)
                                {
                                    Contrecase = Contrecase - 1;
                                    NBRTouch = NBRTouch + 1;
                                    label4.Text = "Nombre de bateaux touchés :" + NBRTouch;
                                    btn.BackColor = Color.Blue;
                                }
                                
                            };
                            
                        }
                    }
                    boatsPlaced++; // ajoute 1 à boatsPlaced pour dir que le Contre-Torpilleur à bien été générer
                }
                else if (!horizontal && y + 1 < 10 && IsPlacementValid(x, y, 3, false)) // si l'emplacement d'avant n'était pas libre
                {
                    for (int i = 0; i < 3; i++)
                    {
                        var btn = tableLayoutPanel1.GetControlFromPosition(x, y + i) as Button;
                        if (btn != null)
                        {
                            btn.Tag = "Contre";
                            btn.ForeColor = Color.Black;
                            boates.Add(btn);
                            ContreList.Add(btn);
                            btn.Click += (s, e) =>
                            {
                                if (btn.BackColor == Color.White){
                                    Contrecase = Contrecase - 1;
                                    NBRTouch = NBRTouch + 1;
                                    label4.Text = "Nombre de bateaux touchés :" + NBRTouch;
                                    btn.BackColor = Color.Blue;
                                }
                            };
                        }

                    }
                    boatsPlaced++;
                }
            }
            if (boatsPlaced == 1) // si le Contre-Torpielle à bien été placé, alors on ajoute 1 au nombre de bateaux placés
            {
                bateaux = bateaux + 1;
            }


        }
        private async void CreateContre1Boats() // Fonctions permettant de généré 1 deuxième Contre-Torpille ( on peut pas placer deux fois le même bateau avec cette fonctions ducoup je fait la fonctions deux fois)
        {

            Random rand = new Random();

            int boatsPlaced = 0;

            while (boatsPlaced < 1)
            {

                await Task.Delay(1);

                int x = rand.Next(0, 10);
                int y = rand.Next(0, 10);
                bool horizontal = rand.Next(0, 2) == 0;

                if (horizontal && x + 1 < 10 && IsPlacementValid(x, y, 3, true))
                {
                    for (int i = 0; i < 3; i++)
                    {
                        var btn = tableLayoutPanel1.GetControlFromPosition(x + i, y) as Button;
                        if (btn != null)
                        {
                            btn.Tag = "Contre1";
                            btn.ForeColor = Color.Black;
                            boates.Add(btn);
                            ContreListe2.Add(btn);

                            btn.Click += (s, e) =>
                            {
                                if (btn.BackColor == Color.White){
                                    Contrecase1 = Contrecase1 - 1;
                                    NBRTouch = NBRTouch + 1;
                                    label4.Text = "Nombre de bateaux touchés :" + NBRTouch;
                                    btn.BackColor = Color.Blue;
                                }
                            };
                        }
                    }
                    boatsPlaced++;
                }
                else if (!horizontal && y + 1 < 10 && IsPlacementValid(x, y, 3, false))
                {
                    for (int i = 0; i < 3; i++)
                    {
                        var btn = tableLayoutPanel1.GetControlFromPosition(x, y + i) as Button;
                        if (btn != null)
                        {
                            btn.Tag = "Contre1";
                            btn.ForeColor = Color.Black;
                            boates.Add(btn);
                            ContreListe2.Add(btn);
                            btn.Click += (s, e) =>
                            {
                                if (btn.BackColor == Color.White){
                                    Contrecase1 = Contrecase1 - 1;
                                NBRTouch = NBRTouch + 1;
                                    label4.Text = "Nombre de bateaux touchés :" + NBRTouch;
                                    btn.BackColor = Color.Blue;
                                }
                            };
                        }

                    }
                    boatsPlaced++;
                }
            }
            if (boatsPlaced == 1)
            {
                bateaux = bateaux + 1;
            }


        }
        private void LoadGams()
        {

        }
        private async void CreateTorpilleurBoats()
        {
            Random rand = new Random();

            int boatsPlaced = 0;

            while (boatsPlaced < 1)
            {

                await Task.Delay(1);

                int x = rand.Next(0, 10);
                int y = rand.Next(0, 10);
                bool horizontal = rand.Next(0, 2) == 0;

                if (horizontal && x + 1 < 10 && IsPlacementValid(x, y, 2, true))
                {
                    for (int i = 0; i < 2; i++)
                    {
                        var btn = tableLayoutPanel1.GetControlFromPosition(x + i, y) as Button;
                        if (btn != null)
                        {
                            btn.Tag = "Torpille";
                            btn.ForeColor = Color.Black;
                            boates.Add(btn);
                            TorpiListe.Add(btn);
                            btn.Click += (s, e) =>
                            {
                                if (btn.BackColor == Color.White){
                                    Torpilleurcase = Torpilleurcase - 1;
                                NBRTouch = NBRTouch + 1;
                                    label4.Text = "Nombre de bateaux touchés :" + NBRTouch;
                                    btn.BackColor = Color.Blue;
                                }
                            };
                        }
                    }
                    boatsPlaced++;
                }
                else if (!horizontal && y + 1 < 10 && IsPlacementValid(x, y, 2, false))
                {
                    for (int i = 0; i < 2; i++)
                    {
                        var btn = tableLayoutPanel1.GetControlFromPosition(x, y + i) as Button;
                        if (btn != null)
                        {
                            btn.Tag = "Torpille";
                            btn.ForeColor = Color.Black;
                            boates.Add(btn);
                            TorpiListe.Add(btn);
                            btn.Click += (s, e) =>
                            {
                                if (btn.BackColor == Color.White){
                                    Torpilleurcase = Torpilleurcase - 1;
                                    NBRTouch = NBRTouch + 1;
                                    label4.Text = "Nombre de bateaux touchés :" + NBRTouch;
                                    btn.BackColor = Color.Blue;
                                }
                            };
                        }
                    }
                    boatsPlaced++;
                }
            }
            if (boatsPlaced == 1)
            {
                bateaux = bateaux + 1;
            }
        }
        private async void CreateAvionBoats()
        {
            Random rand = new Random();

            int boatsPlaced = 0;

            while (boatsPlaced < 1)
            {

                await Task.Delay(1);

                int x = rand.Next(0, 10);
                int y = rand.Next(0, 10);
                bool horizontal = rand.Next(0, 2) == 0;

                if (horizontal && x + 1 < 10 && IsPlacementValid(x, y, 5, true))
                {
                    for (int i = 0; i < 5; i++)
                    {
                        var btn = tableLayoutPanel1.GetControlFromPosition(x + i, y) as Button;
                        if (btn != null)
                        {
                            btn.Tag = "Avions";
                            btn.ForeColor = Color.Black;
                            boates.Add(btn);
                            AvionList.Add(btn);
                            btn.Click += (s, e) =>
                            {
                                if (btn.BackColor == Color.White){
                                    Avionscase = Avionscase - 1;
                                NBRTouch = NBRTouch + 1;
                                    label4.Text = "Nombre de bateaux touchés :" + NBRTouch;
                                    btn.BackColor = Color.Blue;
                                }
                            };
                        }
                    }
                    boatsPlaced++;
                }
                else if (!horizontal && y + 1 < 10 && IsPlacementValid(x, y, 5, false))
                {
                    for (int i = 0; i < 5; i++)
                    {
                        var btn = tableLayoutPanel1.GetControlFromPosition(x, y + i) as Button;
                        if (btn != null)
                        {
                            btn.Tag = "Avions";
                            btn.ForeColor = Color.Black;
                            boates.Add(btn);
                            AvionList.Add(btn);
                            btn.Click += (s, e) =>
                            {
                                if (btn.BackColor == Color.White){
                                    Avionscase = Avionscase - 1;
                                NBRTouch = NBRTouch + 1;
                                    label4.Text = "Nombre de bateaux touchés :" + NBRTouch;
                                    btn.BackColor = Color.Blue;
                                }
                            };
                        }
                    }
                    boatsPlaced++;
                }
            }
            if (boatsPlaced == 1)
            {
                bateaux = bateaux + 1;
            }
        }
        private async void CreateCroiseurBoats()
        {
            Random rand = new Random();

            int boatsPlaced = 0;

            while (boatsPlaced < 1)
            {

                await Task.Delay(1);

                int x = rand.Next(0, 10);
                int y = rand.Next(0, 10);
                bool horizontal = rand.Next(0, 2) == 0;

                if (horizontal && x + 1 < 10 && IsPlacementValid(x, y, 4, true))
                {
                    for (int i = 0; i < 4; i++)
                    {
                        var btn = tableLayoutPanel1.GetControlFromPosition(x + i, y) as Button;
                        if (btn != null)
                        {
                            btn.Tag = "Croiseur";
                            btn.ForeColor = Color.Black;
                            boates.Add(btn);
                            CroiseurList.Add(btn);
                            btn.Click += (s, e) =>
                            {
                                if (btn.BackColor == Color.White){
                                    croiseurcase = croiseurcase - 1;
                                NBRTouch = NBRTouch + 1;
                                    label4.Text = "Nombre de bateaux touchés :" + NBRTouch;
                                    btn.BackColor = Color.Blue;
                                }
                            };
                        }
                    }
                    boatsPlaced++;
                }
                else if (!horizontal && y + 1 < 10 && IsPlacementValid(x, y, 4, false))
                {
                    for (int i = 0; i < 4; i++)
                    {
                        var btn = tableLayoutPanel1.GetControlFromPosition(x, y + i) as Button;
                        if (btn != null)
                        {
                            btn.Tag = "Croiseur";
                            btn.ForeColor = Color.Black;
                            boates.Add(btn);
                            CroiseurList.Add(btn);
                            btn.Click += (s, e) =>
                            {
                                if (btn.BackColor == Color.White){
                                    croiseurcase = croiseurcase - 1;
                                NBRTouch = NBRTouch + 1;
                                    label4.Text = "Nombre de bateaux touchés :" + NBRTouch;
                                    btn.BackColor = Color.Blue;
                                }
                            };
                        }
                    }
                    boatsPlaced++;
                }
            }
            if (boatsPlaced == 1)
            {
                bateaux = bateaux + 1;
            }
        }
        private bool IsPlacementValid(int x, int y, int length, bool horizontal) // Fonction qui vérifie si l'emplacement choisi aléatoirement est libre et aussi qu'il y ai un d'espace entre lui et un autre bateau
        {
            for (int i = -1; i <= length; i++) // tant que "i" est plus petit que la longueur
            {
                for (int j = -1; j <= 1; j++)
                {
                    int checkX = horizontal ? x + i : x + j; 
                    int checkY = horizontal ? y + j : y + i;

                    if (checkX < 0 || checkX >= 10 || checkY < 0 || checkY >= 10){ // vérifie si il y a un bateau trop proche ou pas
                        return false; // si il retourn false c'est tout bon, sinon il vas au prochaine "if" pour changer de point
                    }

                    Control ctrl = tableLayoutPanel1.GetControlFromPosition(checkX, checkY);
                    if (ctrl != null && ctrl.ForeColor == Color.Black){ // Il y a déjà un bateau ou trop proche
                        return false; // si il retourn false c'est tout bon sinon, il sort de la boucle et retrouve un point
                    }
                }
            }
            return true; // doit retrouver un point
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
        }

        protected override CreateParams CreateParams // améliore la fluidité
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000; // pour un rendu fluide
                return cp;

            }
        }

        private async void timer2_Tick(object sender, EventArgs e) // vérifie si un bateau entier à été touché (donc il devient noir)
        {
            foreach (Button x in ContreList)
            {
                if (Contrecase == 0) // si tout les contre-torpille on été touché, tout le bateau devient noir et ajoute 1 au nombre de bateaux coulés
                {
                    x.BackColor = Color.Black;
                    if (bt1 == false ){ // si on a pas encore ajouté 1 au nombre de bateaux coulés
                        bt1 = true;
                        NBRcoulé = NBRcoulé + 1;
                        label6.Text = "Nombre de bateaux coulés : " + NBRcoulé;
                    }
                }
            }
            foreach (Button x in ContreListe2) 
            {
                if (Contrecase1 == 0)
                {
                    x.BackColor = Color.Black;
                    if (bt2 == false){
                        bt2  = true;
                        NBRcoulé = NBRcoulé + 1;
                        label6.Text = "Nombre de bateaux coulés : " + NBRcoulé;
                    }
                }
            }
            foreach (Button x in CroiseurList)
            {
                if (croiseurcase == 0)
                {
                    x.BackColor = Color.Black;
                    if (bt3 == false){
                        bt3 = true;
                    NBRcoulé = NBRcoulé + 1;
                        label6.Text = "Nombre de bateaux coulés : " + NBRcoulé;
                    }
                }
            }
            foreach (Button x in AvionList)
            {
                if (Avionscase == 0)
                {
                    x.BackColor = Color.Black;
                    if (bt4 == false){
                        bt4 = true;
                    NBRcoulé = NBRcoulé + 1;
                        label6.Text = "Nombre de bateaux coulés : " + NBRcoulé;
                    }
                }
            }
            foreach (Button x in TorpiListe)
            {
                if (Torpilleurcase == 0)
                {
                    x.BackColor = Color.Black;
                    if (bt5 == false){
                        bt5 = true;
                    NBRcoulé = NBRcoulé + 1;
                        label6.Text = "Nombre de bateaux coulés : " + NBRcoulé;
                    }
                }
            }
            if (bateaux == 5)
            {
                panel2.Visible = false;
            }
            if (Torpilleurcase == 0 && Avionscase == 0 && croiseurcase == 0 && Contrecase1 == 0 && Contrecase == 0 && win == false) // si tout les bateaux on été coulés
            {
                win = true;
                MessageBox.Show("Vous avez gagné !!!"); // message de victoire
                string fileName = path + @"\Victoire.txt";
                if (File.Exists(fileName) == false) // crée le fichier pour que TryToEscape! puisse savoir que le joueur à gagner à la bataille navale et puisse changer d'épreuve
                {
                    File.Create(fileName).Close();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e) // si on clique sur le bouton "restart"
        {
            DialogResult rn = MessageBox.Show("Voulez vous vraiment TOUT recommencer ?", "Recommencer", MessageBoxButtons.YesNo);
            if (rn == DialogResult.Yes) 
            {
                MessageBox.Show("pourquoi...");
                MessageBox.Show("TANT QUE TU RESTE DANS LE JEU çA ME VAS ! :3");
                Application.Restart(); // relance le jeu
            }
            if (rn == DialogResult.No)
            {
                MessageBox.Show("Très bien, GAGNEZ MOI CETTE PARTIE !");
            }
        }

        private void button2_Click(object sender, EventArgs e) // si on clique sur le bouton "Quitter"
        {
            DialogResult rn = MessageBox.Show("Voulez vous vraiment ARRÊTER de jouer ?", "Quitter", MessageBoxButtons.YesNo);
            if (rn == DialogResult.Yes)
            {
                MessageBox.Show("Bien... passez une bonne journée...");
                MessageBox.Show("...");
                MessageBox.Show(":(");
                Application.Exit(); // Ferme le jeu
            }
            if (rn == DialogResult.No)
            {
                MessageBox.Show("J'ÉTAIS SUR QUE TU N'ALLAIS PAS ABANDONNER ! :3");
                MessageBox.Show(":)");
                MessageBox.Show("^^");
            }
        }

        private void button6_Click(object sender, EventArgs e) // trouve un bateau aléatoirement pour aider
        {
            if (aide == false && win == false) // si on a encore 1 point d'aide
            {
                
                label5.Text = "Points d'aide : 0";
                while (aide != true) // tant que le point d'aide n'a pas encore touché un bateau
                {
                    int randIndex = rpd.Next(boates.Count);
                    Button random = boates[randIndex];
                    if (random != null && random.BackColor == Color.White) // vérifie si le bateau pris aléatoirement n'as pas déjà été touché
                    {
                        random.BackColor = Color.Blue;
                        if (random.Tag == "Contre")
                        {
                            Contrecase = Contrecase - 1;
                        }
                        if (random.Tag == "Contre1")
                        {
                            Contrecase1 = Contrecase - 1;
                        }
                        if (random.Tag == "Torpille")
                        {
                            Torpilleurcase = Torpilleurcase - 1;
                        }
                        if (random.Tag == "Croiseur")
                        {
                            croiseurcase = croiseurcase - 1;
                        }
                        if (random.Tag == "Avions")
                        {
                            Avionscase = Avionscase - 1;
                        }
                        NBRTouch = NBRTouch + 1;
                        label4.Text = "Nombre de bateaux touchés :" + NBRTouch;
                        aide = true;
                    }
                }
                
                return;
            }
            if (aide == true && win == false) // si on a plus de points d'aide
            {
                MessageBox.Show("Vous n'avez plus de points d'aide !");
            }
            if (win == true) // si on a déjà gagner le jeu
            {
                MessageBox.Show("Tu fais exprès ?!?!? ");
                MessageBox.Show("Il n'y a plus de bateau parce que tu asssssssssss.....");
                MessageBox.Show("GAGNER !");
                MessageBox.Show("maintenant relance une partie sur mon MAGNIFIQUE jeu");
                MessageBox.Show(":3");
                MessageBox.Show("J'ai pas confiance en TOI ducoup je relance le jeu à t'as place. Bisous à plus !");
                Application.Restart();
            }
            
        }

        private void label4_Click(object sender, EventArgs e)
        {
            
        }
    }
}
