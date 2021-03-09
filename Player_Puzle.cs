using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;


public class Player_Puzle : MonoBehaviour
{
    [Header("РУЧНАЯ УСТ - !ВНИМАНИЕ")]
    public GameObject Puzle_Cube_Prefab;
    public LayerMask mask;
    public Material[] Mat_List;
    public Texture[] _Sprites = new Texture[6];//в инспекторе можно расширить
    //Эффекты
    public GameObject StartEffects,NameChekEffect;
    [Header("АВТО УСТ")]
    public GameObject CloneEffects;

    public GameObject _cam;
    public float _FieldOvView;
    public float t;

    public GameObject[] SupportCube_Massive;//Массив вспомогательных кубов
    public List<Transform> Puzle_List;//Рабочие пазлы
    public int RandomPos_Ready;
    public GameObject Clone;
    public GameObject Respawn;
    public GameObject Select_0, Select_1;//обьекты выделения
    public float Timer_Respawn, Timer_CoulDown, Timer_Mission,Timer_Result;
    public int GamePhase;

    public Vector3[] Grid;
    public Transform Grid_StartPos;
    public bool GridGizmos;
    //audio
    public AudioClip VictoryTheme;
    public AudioSource Music;

    [Header("CANVAS AUTO")]
    public Text TimerText;
    public Text ScoreText;
    public Text ScoreInvoke_Text;
    //
    public float Score;

    [Header("EXTRA AUTO")]
    public Renderer rend;
    [HideInInspector] public int ColorCharger_Int, Red, Blue, Green, Yellow, IndianaJohns, Pyramid;
    [HideInInspector] public string Color_Name = "";

    //Experimental
    public bool EXP_On;
    public GameObject Zero,One;
    public GameObject ParticleSystem;
    public Vector3 V3_RGBA;
    public Renderer Gizmos_Rend;
    // Start is called before the first frame update
    void Start()
    {
        //SupportCube_Massive = GameObject.FindGameObjectsWithTag("Support_Cube");
        Respawn = GameObject.Find("Respawn");
        _cam = GameObject.Find("Main Camera");
        //Canvas
        TimerText = GameObject.Find("Timer").GetComponent<Text>();
        ScoreText = GameObject.Find("Score_Update").GetComponent<Text>();
        ScoreInvoke_Text = GameObject.Find("Score_Invoke").GetComponent<Text>();

        GameObject.Find("Canvas").transform.Find("RestartMenu").transform.Find("Restart_Button").GetComponent<Button>().onClick.AddListener(Restart);
        GameObject.Find("Canvas").transform.Find("RestartMenu").transform.gameObject.SetActive(false);
        Music = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        if (GamePhase == 0)//Растановка элементов
        {
            Timer_Respawn += Time.deltaTime;
            if (Puzle_List.Count < 16 && Timer_Respawn >= 0.01f)
            {
                Clone = Instantiate(Puzle_Cube_Prefab, Respawn.transform.position, Respawn.transform.rotation);
                if (Clone.AddComponent<Cube_Moving>() == false) { Clone.AddComponent<Cube_Moving>(); }
            }
            int Random_Int = 0;
            Random_Method();//Получаем случайную позицию для пазла
            void Random_Method()
            {
                Random_Int = Random.Range(0, SupportCube_Massive.Length);
                if (SupportCube_Massive[Random_Int].transform.name.Contains("V3") == true)
                {
                    Random_Method();
                }

            }
            if (Clone != null && Clone.GetComponent<Cube_Moving>() != null)//Установка случ позиции
            {
                for (int i = 0; i < SupportCube_Massive.Length; i++)
                {
                    //if (SupportCube_Massive[i].transform.name.Contains("V3") == false && Puzle_List.Contains(Clone.transform) == false)
                    if (SupportCube_Massive[i].transform.name.Contains("V3") == false && Puzle_List.Contains(Clone.transform) == false && i == Random_Int)
                    {
                        //Работа с клоном
                        Clone.GetComponent<Cube_Moving>().NewPos = SupportCube_Massive[i].transform.position;
                        Clone.transform.Find("Cloacker").transform.gameObject.SetActive(false);
                        SupportCube_Massive[i].transform.name += "V3";//Чтобы небыло пофторного наложения
                        if (ColorCharger_Int == 0) { Color_Name = "Red"; Set_Sprtite(ref Red, ref ColorCharger_Int, ref _Sprites[0], new Vector4(1f, 0f, 0f, 1f),ref Color_Name);}
                        if (ColorCharger_Int == 1) { Color_Name = "Blue"; Set_Sprtite(ref Blue, ref ColorCharger_Int, ref _Sprites[1], new Vector4(0f, 0f, 1, 1f), ref Color_Name); }
                        if (ColorCharger_Int == 2) { Color_Name = "Green"; Set_Sprtite(ref Green, ref ColorCharger_Int, ref _Sprites[2], new Vector4(0f, 1f, 0f, 1f),ref Color_Name); }
                        if (ColorCharger_Int == 3) { Color_Name = "Yellow"; Set_Sprtite(ref Yellow, ref ColorCharger_Int, ref _Sprites[3], new Vector4(1f, 0.92f, 0.016f, 1f),ref Color_Name); }
                        if (ColorCharger_Int == 4) { Color_Name = "Pyramid"; Set_Sprtite(ref Pyramid, ref ColorCharger_Int, ref _Sprites[4], new Vector4(0f, 0.66f, 0.7f, 1f),ref Color_Name); }
                        if (ColorCharger_Int >= 5) { IndianaJohns = 0; Color_Name = "IndianaJohns"; Set_Sprtite(ref IndianaJohns, ref ColorCharger_Int, ref _Sprites[5], new Vector4(0.7f, 0f, 0.3f, 0f), ref Color_Name); }

                        Puzle_List.Add(Clone.transform);//Добавляем 
                    }
                }
                Clone = null;
                Timer_Respawn = 0;
            }
            if (Puzle_List.Count == 16)
            {
                Timer_Mission = 4f;//на демонстрацию иконок
                GamePhase = 1;
            }
        }



        if (GamePhase == 1)
        {
            //Камера
            t += Time.deltaTime * 0.5f;
            _FieldOvView = Mathf.Lerp(25, 21, t);
            _cam.GetComponent<Camera>().orthographicSize = _FieldOvView;
            //Смена фаз. Включение блокировки
            Timer_Mission -= Time.deltaTime;
            if(Timer_Mission <= 0f)
            {
                for(int i = 0; i<Puzle_List.Count; i++)
                {
                    Puzle_List[i].transform.Find("Cloacker").transform.gameObject.SetActive(true);
                }
                if(StartEffects != null)
                {
                    StartEffects.SetActive(true);
                    Destroy(StartEffects, 0.7f);
                }

                Timer_Mission = 60f;
                GamePhase = 2;
            }
        }




        if (GamePhase == 2)
        {
            if (Timer_CoulDown > 0)//NameChek - Set
            {
                Timer_CoulDown -= Time.deltaTime;
            }
            if (Timer_CoulDown <= 0)
            {
                if ((Select_0 == null || Select_1 == null) && Input.touchCount == 1)
                {
                    Select_Puzle();
                }
                if (Select_0 != null && Select_1 != null)
                {
                    Name_Check();
                }
            }
            if(Puzle_List.Count > 0)
            {
                Timer_Mission -= Time.deltaTime;
                TimerText.text = Mathf.Floor(Timer_Mission).ToString();
            }

            if (Timer_Mission <= 0 || Puzle_List.Count == 0)
            {
                Result();
            }
        }
    }


    private void OnDrawGizmos()
    {
        Ray _raySo;
        RaycastHit _hitSo;

        _raySo = Camera.main.ScreenPointToRay(Input.mousePosition);
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(_raySo);
        if (Physics.Raycast(_raySo, out _hitSo, 10000.0f, mask))
        {
            //Debug.Log(_hitSo.transform.name);
        }
        if (EXP_On == true)
        {
            F1(ref Zero,ref _Sprites[5],new Vector4(0.3f,0.3f,0.3f,1f));
            F1(ref One, ref _Sprites[2], new Vector4(0.0f, 1f, 0.5f, 1f));
            void F1(ref GameObject Obj,ref Texture Image_S, Vector4 RGBA)
            {
                Gizmos_Rend = Obj.GetComponent<MeshRenderer>();
                Material[] Gizmo_Mat = Gizmos_Rend.materials;
                Gizmo_Mat[0].SetTexture("Texture_0", Image_S);
                if (Gizmo_Mat[1] != null)
                {
                    Gizmo_Mat[1].SetColor("Color_0", RGBA * 2);
                }
                Gizmos_Rend.materials = Gizmo_Mat;
            }
            Gizmos_Rend = ParticleSystem.GetComponent<Renderer>();
            float floatX = Random.Range(0f, 1f);
            float floatY = Random.Range(0f, 1f);
            float floatZ = Random.Range(0f, 1f);
            V3_RGBA = new Vector4(floatX, floatY, floatZ);
            Gizmos_Rend.material.SetVector("Color_0", V3_RGBA * 5);
        }
        if(GridGizmos == true)//Создаем GRID Решетку
        {
            Grid_StartPos = GameObject.Find("Start_Pos").transform;
            int Upline = 0;
            int Righline = 0;
            int GridSize = 4;//4 клетки на ГРИД
            float Block_Rad = 4.7f;
            SupportCube_Massive = GameObject.FindGameObjectsWithTag("Support_Cube");
            for (int i = 0; i < SupportCube_Massive.Length; i++)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(SupportCube_Massive[i].transform.position, Block_Rad);

                Transform Obj = SupportCube_Massive[i].transform;
                Obj.transform.position = new Vector3(Grid_StartPos.position.x + 2.1f * Block_Rad * Righline, Obj.position.y, Grid_StartPos.position.z + 2.1f * Block_Rad * Upline);
                Righline += 1;
                if(Righline == GridSize)
                {
                    Upline += 1;
                    Righline = 0;
                }

            }
        }
    }
    void Restart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Scene_Puzle");
    }
    void Select_Puzle()
    {
        Ray _raySolo;
        RaycastHit _hitSolo;
        _raySolo = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);

        if (Physics.Raycast(_raySolo, out _hitSolo, 10000.0f, mask))
        {
            if (Select_0 == null && _hitSolo.transform.tag == "Puzle" && _hitSolo.transform.gameObject != Select_0)
            {
                M2(ref Select_0);
            }

            if (Select_1 == null && _hitSolo.transform.tag == "Puzle" && _hitSolo.transform.gameObject != Select_1 && _hitSolo.transform.gameObject != Select_0)
            {
                M2(ref Select_1);

            }
            void M2(ref GameObject Obj)
            {
                Obj = _hitSolo.transform.gameObject;
                Obj.transform.Find("Cloacker").transform.gameObject.SetActive(false);
                string ColorName = Obj.transform.name;
                //Color_Set(ref Obj, ref _Sprites[6],ref ColorName);
            }
        }
    }
    void Set_Sprtite(ref int Color_Int, ref int ColorCharger_S, ref Texture Image_S, Vector4 RGBA, ref string Color_Name)
    {
        if (Color_Int >= 2)
        {
            ColorCharger_S += 1;
        }
        if (Color_Int <= 1)
        {
            Color_Set(ref Clone,ref Image_S, ref Color_Name);
        }
        Color_Int += 1;
    }

    void Color_Set(ref GameObject CloneObj, ref Texture Image_S, ref string Color_Name)
    {
        Vector4 RGBA = new Vector4(1, 1, 1, 1);
        //Выбор RGBA
        if (Color_Name.Contains("Red") == true) { RGBA =  new Vector4(1f, 0f, 0f, 1f); }
        if (Color_Name.Contains("Blue") == true) { RGBA =  new Vector4(0f, 0f, 1, 1f); }
        if (Color_Name.Contains("Green") == true) { RGBA = new Vector4(0f, 1f, 0f, 1f); }
        if (Color_Name.Contains("Yellow") == true) { RGBA = new Vector4(1f, 0.92f, 0.016f, 1f); }
        if (Color_Name.Contains("Pyramid") == true) { RGBA =  new Vector4(0f, 0.66f, 0.7f, 1f); }
        if (Color_Name.Contains("IndianaJohns") == true) { RGBA = new Vector4(0.7f, 0f, 0.3f, 0f); }
        //Для Puzle
        if (CloneObj != CloneEffects)
        {
            rend = CloneObj.GetComponent<MeshRenderer>();
            rend.materials = Mat_List;//Присваеваем первичные материалы
            Material[] Gizmo_Mat = rend.materials;//для кастома по отдельности

            if (Image_S != null)
            {
                Gizmo_Mat[0].SetTexture("Texture_0", Image_S);
            }

            if (Gizmo_Mat[1] != null)
            {
                Gizmo_Mat[1].SetColor("Color_0", RGBA * 4);
            }
            rend.materials = Gizmo_Mat;
        }
        //обработка для ParticleSystem
        if (CloneObj == CloneEffects)
        {
            rend = CloneObj.GetComponent<Renderer>();
            rend.material.SetColor("Color_0", RGBA * 8f);
        }
        CloneObj.transform.name += Color_Name;
    }

    void Name_Check()//Сравнение Имен Select_0 Select_1
    {
        if (Select_0 != null && Select_0.transform.name.Contains("Red") == true) { Support_Check("Red"); }
        if (Select_0 != null && Select_0.transform.name.Contains("Blue") == true) { Support_Check("Blue"); }
        if (Select_0 != null && Select_0.transform.name.Contains("Green") == true) { Support_Check("Green"); }
        if (Select_0 != null && Select_0.transform.name.Contains("Yellow") == true) { Support_Check("Yellow"); }
        if (Select_0 != null && Select_0.transform.name.Contains("IndianaJohns") == true) { Support_Check("IndianaJohns"); }
        if (Select_0 != null && Select_0.transform.name.Contains("Pyramid") == true) { Support_Check("Pyramid"); }

        void Support_Check(string Name_S)
        {
            if (Select_1.transform.name.Contains(Name_S) == false)//если отличаются
            {
                Invoke("Invoke_Name_Check_Cloacker_On", 1.5f);
            }
            if (Select_1.transform.name.Contains(Name_S) == true)//если схожи
            {
                M1(ref Select_0);
                M1(ref Select_1);
                void M1(ref GameObject Obj)
                {
                    Puzle_List.Remove(Obj.transform);
                    Destroy(Obj, 0.1f);
                    //Color_Reset(ref Obj);
                    //Effects
                    CloneEffects = Instantiate(NameChekEffect, Obj.transform.Find("Effect_Respawn").transform.position, Obj.transform.Find("Effect_Respawn").transform.rotation);
                    Color_Set(ref CloneEffects, ref _Sprites[6], ref Name_S);
                    Destroy(CloneEffects, 1f);
                    Obj = null;
                }
                Score += 200;
                ScoreInvoke_Text.text = "+200";
                Invoke("Invoke_Name_Check_Score_Invoke_Off", 0.7f);

            }
            ScoreText.text = Score.ToString();
            Timer_CoulDown = 1.5f;
        }
    }

    public void Invoke_Name_Check_Cloacker_On()
    {
        K1(ref Select_0);
        K1(ref Select_1);
        void K1(ref GameObject Obj)
        {
            if (Obj != null)
            {
                Obj.transform.Find("Cloacker").transform.gameObject.SetActive(true);
                //Color_Reset(ref Obj);
                Obj = null;
            }
        }
    }

    public void Invoke_Name_Check_Score_Invoke_Off()
    {
        ScoreInvoke_Text.text = "";
    }

        void Color_Reset(ref GameObject WorkObj)//Сброс цвета на дефолтный белый
    {
        if (WorkObj != null)
        {
            rend = WorkObj.GetComponent<MeshRenderer>();
            Mat_List = rend.materials;
            if (Mat_List[1] != null)
            {
                Mat_List[1].SetColor("Color_0", new Vector4(1f, 1f, 1f, 1f));
            }
            rend.materials = Mat_List;
        }
    }


    void Result()
    {
        GameObject.Find("Canvas").transform.Find("RestartMenu").transform.gameObject.SetActive(true);
        if (Puzle_List.Count == 0)
        {

            //подсчет очков
            Timer_Result += Time.deltaTime;
            int TimeBonus = 0;
            if (Timer_Mission > 0) { TimeBonus = 2; }
            if (Timer_Mission >= 20) { TimeBonus = 3; }
            if (Timer_Mission >= 40) { TimeBonus = 5; }
            float ResultScore = Mathf.Lerp(0f, Score * TimeBonus, Timer_Result * 0.5f);
            ResultScore = Mathf.Floor(ResultScore);
            GameObject.Find("Canvas").transform.Find("RestartMenu").transform.Find("Score").GetComponent<Text>().text = ResultScore.ToString();
            GameObject.Find("Canvas").transform.Find("RestartMenu").transform.Find("Time_Result").GetComponent<Text>().text = Mathf.Floor(60f - Timer_Mission).ToString();
            GameObject.Find("Canvas").transform.Find("RestartMenu").transform.Find("Bonus_Result").GetComponent<Text>().text = "x" + TimeBonus.ToString();

            //Статус миссии
            GameObject.Find("Canvas").transform.Find("RestartMenu").transform.Find("Mission_Result").GetComponent<Text>().text = "Mission Succesion";
            if(Music.clip != VictoryTheme)
            {
                Music.clip = VictoryTheme;
                Music.Play();
            }
        }
        if (Timer_Mission <= 0)
        {
            GameObject.Find("Canvas").transform.Find("RestartMenu").transform.Find("Time_Result").GetComponent<Text>().text = "0";
            GameObject.Find("Canvas").transform.Find("RestartMenu").transform.Find("Bonus_Result").GetComponent<Text>().text = "0";
            GameObject.Find("Canvas").transform.Find("RestartMenu").transform.Find("Score").GetComponent<Text>().text = "0";//подсчет очков
            GameObject.Find("Canvas").transform.Find("RestartMenu").transform.Find("Mission_Result").GetComponent<Text>().text = "Mission Failed";//Статус миссии
        }
    }
}
