 void Start()
    {
        Button_Add_V();
    }

    void Button_Add_V()
    {
        Object_S();
        Main_Menu_S();
        Ai_Lvl_Menu();
        Game_Menu_S();
        Result_Menu_S();
        Privacy_Policy_Menu_S();
        Invoke_Shop_Check();
        Json_Settings_Save();
        Json_Settings_Load();
        //Настройки
        //-Всомогательные обьекты
        void Object_S()
        {
            Respawn = GameObject.Find("Respawn");
            _Camera = GameObject.Find("Main Camera");
            Main_Menu_Decor = GameObject.Find("Main_Menu_Decor");
            GamePhase = -1;
        }
        //-Главное меню
        void Main_Menu_S()
        {
            if (_JsonSave.Privacy_Policy_Lvl == 0)
            {
                PrivacyPolicy_BackGround_S();
            }
            GameObject.Find("Canvas").transform.Find("Main_BackGround").transform.Find("Start_Btn").GetComponent<Button>().onClick.AddListener(Ai_Lvl_BackGround_S);
            GameObject.Find("Canvas").transform.Find("Main_BackGround").transform.Find("No_Ads_Btn").GetComponent<Button>().onClick.AddListener(Shop.Buy_DeluxeVersion);
            GameObject.Find("Canvas").transform.Find("Main_BackGround").transform.Find("Privacy_Policy_Btn").GetComponent<Button>().onClick.AddListener(PrivacyPolicy_BackGround_S);
            GameObject.Find("Canvas").transform.Find("Main_BackGround").transform.Find("Reset_Btn").GetComponent<Button>().onClick.AddListener(Reset_Settings_V);
        }

        void Ai_Lvl_Menu()
        {
            GameObject.Find("Canvas").transform.Find("Ai_Lvl_BackGround").transform.Find("4x4_Btn").GetComponent<Button>().onClick.AddListener(Grid_4x4);
            GameObject.Find("Canvas").transform.Find("Ai_Lvl_BackGround").transform.Find("6x6_Btn").GetComponent<Button>().onClick.AddListener(Grid_6x6);

            void Grid_4x4()
            {
                Grid_Size_S(4);
            }

            void Grid_6x6()
            {
                Grid_Size_S(6);
            }

            void Grid_Size_S(int Grid_Size_S)
            {
                GridSize = Grid_Size_S;
                Game_BackGround_S();
                Main_Menu_Decor.transform.gameObject.SetActive(false);
                //Формирование сетки
                GRID_Clear();
                GRID_Settings_V();

                //формирование рабочих текстур и ячеек
                Int_List_Null_Adder_S(ref Work_Texture_Int_List, _Texture_Massive.Length);
                Int_List_Null_Adder_S(ref Random_Pos_List, Mathf.FloorToInt(GridSize * GridSize));
                GamePhase = 0;

            }
        }

        //-Меню в игре
        void Game_Menu_S()
        {
            TimerText = GameObject.Find("Canvas").transform.Find("Game_BackGround").transform.Find("Timer").GetComponent<Text>();
            ScoreText = GameObject.Find("Canvas").transform.Find("Game_BackGround").transform.Find("Score_Update").GetComponent<Text>();
            ScoreInvoke_Text = GameObject.Find("Canvas").transform.Find("Game_BackGround").transform.Find("Score_Invoke").GetComponent<Text>();
        }

        //-Меню результатов
        void Result_Menu_S()
        {
            GameObject.Find("Canvas").transform.Find("Reuslt_BackGround").transform.Find("Restart_Button").GetComponent<Button>().onClick.AddListener(Restart_Btn_S);
            GameObject.Find("Canvas").transform.Find("Reuslt_BackGround").transform.gameObject.SetActive(false);

            void Restart_Btn_S()
            {
                UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Scene_Puzle");
            }
        }

        void Privacy_Policy_Menu_S()
        {
            GameObject.Find("Canvas").transform.Find("PrivacyPolicy_BackGround").transform.Find("Next_Page_Btn").GetComponent<Button>().onClick.AddListener(Privacy_Policy_Next_Page);
            GameObject.Find("Canvas").transform.Find("PrivacyPolicy_BackGround").transform.Find("Back_Page_Btn").GetComponent<Button>().onClick.AddListener(Privacy_Policy_Back_Page);
            GameObject.Find("Canvas").transform.Find("PrivacyPolicy_BackGround").transform.Find("I_Agree").GetComponent<Button>().onClick.AddListener(Privacy_Policy_Ok);
            GameObject.Find("Canvas").transform.Find("PrivacyPolicy_BackGround").transform.Find("Cancel").GetComponent<Button>().onClick.AddListener(PrivacyPolicy_Cancel_BackGround);

            //Во вспомогательном меню
            GameObject.Find("Canvas").transform.Find("PrivacyPolicy_BackGround").transform.Find("PrivacyPolicy_Cancel_BackGround").transform.Find("Ok_Btn").GetComponent<Button>().onClick.AddListener(PrivacyPolicy_BackGround_S);

            void Privacy_Policy_Ok()
            {
                Main_BackGround_S();
                _JsonSave.Privacy_Policy_Lvl = 1;
                Json_Settings_Save();
            }

            void Privacy_Policy_Next_Page()
            {
                Privacy_Policy_Page += 1;
                Privacy_Policy_Other_Page();
            }

            void Privacy_Policy_Back_Page()
            {
                Privacy_Policy_Page -= 1;
                Privacy_Policy_Other_Page();
            }

            void PrivacyPolicy_Cancel_BackGround()
            {
                BackGround_Change(GameObject.Find("Canvas").transform, 3, "PrivacyPolicy_BackGround", "PrivacyPolicy_Cancel_BackGround");
            }

            void Privacy_Policy_Other_Page()
            {
                if (Privacy_Policy_Page >= 3) { Privacy_Policy_Page = 3; }
                if (Privacy_Policy_Page <= 0) { Privacy_Policy_Page = 0; }

                GameObject.Find("Canvas").transform.Find("PrivacyPolicy_BackGround").transform.Find("PPM_Page_0").transform.gameObject.SetActive(false);
                GameObject.Find("Canvas").transform.Find("PrivacyPolicy_BackGround").transform.Find("PPM_Page_1").transform.gameObject.SetActive(false);
                GameObject.Find("Canvas").transform.Find("PrivacyPolicy_BackGround").transform.Find("PPM_Page_2").transform.gameObject.SetActive(false);
                GameObject.Find("Canvas").transform.Find("PrivacyPolicy_BackGround").transform.Find("PPM_Page_3").transform.gameObject.SetActive(false);

                if (Privacy_Policy_Page == 0) { GameObject.Find("Canvas").transform.Find("PrivacyPolicy_BackGround").transform.Find("PPM_Page_0").transform.gameObject.SetActive(true); }
                if (Privacy_Policy_Page == 1) { GameObject.Find("Canvas").transform.Find("PrivacyPolicy_BackGround").transform.Find("PPM_Page_1").transform.gameObject.SetActive(true); }
                if (Privacy_Policy_Page == 2) { GameObject.Find("Canvas").transform.Find("PrivacyPolicy_BackGround").transform.Find("PPM_Page_2").transform.gameObject.SetActive(true); }
                if (Privacy_Policy_Page == 3) { GameObject.Find("Canvas").transform.Find("PrivacyPolicy_BackGround").transform.Find("PPM_Page_3").transform.gameObject.SetActive(true); }
            }
        }

        //Для работы с меню
        void Main_BackGround_S()
        {
            BackGround_Change(GameObject.Find("Canvas").transform, 0, "Main_BackGround", "");
        }

        void Ai_Lvl_BackGround_S()
        {
            BackGround_Change(GameObject.Find("Canvas").transform, 0, "Ai_Lvl_BackGround", "");
        }

        void Game_BackGround_S()
        {
            BackGround_Change(GameObject.Find("Canvas").transform, 0, "Game_BackGround", "");
        }

        void PrivacyPolicy_BackGround_S()
        {
            BackGround_Change(GameObject.Find("Canvas").transform, 0, "PrivacyPolicy_BackGround", "");
            BackGround_Change(GameObject.Find("Canvas").transform, 4, "PrivacyPolicy_BackGround", "PrivacyPolicy_Cancel_BackGround");
            if (_JsonSave.Privacy_Policy_Lvl == 1)
            {
                GameObject.Find("Canvas").transform.Find("PrivacyPolicy_BackGround").transform.Find("Cancel").transform.gameObject.SetActive(false);
                GameObject.Find("Canvas").transform.Find("PrivacyPolicy_BackGround").transform.Find("I_Agree").GetComponentInChildren<Text>().text = "Ok";
            }
        }
    }

