using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class tray : MonoBehaviour
{
    GameObject moneyPlus_particle;
    traySpawner spawner;
    GM gm;
    public Sprite food;
    public string provName;
    public Transform prov;
    Image food_notepad;
    Image food_notepad2;
    //TextMeshProUGUI text_notepad;
    [SerializeField] private GameObject penanda;
    [SerializeField] private Image[] img;
    public int coins = 0;
    Animator anim;
    [SerializeField] private Animator anim2;
    [SerializeField] private notepad note;
    public bool isCooking = false;
    public bool restriction = false;
    public List<string> restrictions;
    public bool done = false;
    //public static bool isClicked;
    // Start is called before the first frame update
    void Start()
    {
        moneyPlus_particle = GameObject.Find("UI").transform.GetChild(0).gameObject;
        prov = GameObject.Find("sprites").transform.Find(provName);
        food_notepad = GameObject.Find("FOOD_NOTEPAD").GetComponent<Image>();
        food_notepad2 = GameObject.Find("FOOD_NOTEPAD2").GetComponent<Image>();
        //text_notepad = GameObject.Find("TEXT_NOTEPAD").GetComponent<TextMeshProUGUI>();
        //tray.isClicked = false;
        anim = GetComponent<Animator>();
        anim2 = GameObject.Find("NOTEPAD").GetComponent<Animator>();
        note = FindObjectOfType<notepad>();
        penanda = GameObject.Find("penanda");
        gm = FindObjectOfType<GM>();
        spawner = FindObjectOfType<traySpawner>();
        if(food.name != "bajigur" && food.name != "esdawet")
        {
            img[0].sprite = food;
            img[0].gameObject.SetActive(true);
            img[1].gameObject.SetActive(false);
            food_notepad.sprite = food;
            food_notepad.enabled = true;
            food_notepad2.enabled = false;
        }
        else
        {
            img[1].sprite = food;
            img[1].gameObject.SetActive(true);
            img[0].gameObject.SetActive(false);
            food_notepad2.sprite = food;
            food_notepad.enabled = false;
            food_notepad2.enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("cooking", isCooking);
        anim.SetBool("done", done);
    }
    bool clicked2 = false;
    public void clicked()
    {
        if (!penanda.GetComponent<Image>().enabled && !isCooking)
        {
            spawner.curr_tray = this;
            if(!restriction)
            {
                gm.canShip = true;
                anim.SetBool("clicked", true);
                penanda.GetComponent<Image>().enabled = true;
            }
            else if (restriction && note.order != this)
            {
                anim.SetBool("clicked",true);
                //tray.isClicked = true;
            }
        }
    }
    public void ship()
    {
        penanda.GetComponent<Image>().enabled = false;
        gm.canShip = false;
        anim.SetBool("clicked", false);
        gm.spawning(prov, food, this);
        //tray.isClicked = false;
        //penanda.GetComponent<Image>().enabled = true;
    }
    public void callNotePad()
    {
        if(restriction)
        {
            anim2.SetBool("in", true);
            note.order = this;
        }
    }
    public void exitNotePad()
    {
        if (note.order == this)
        {
            gm.canShip = false;
            note.order = null;
            anim2.SetBool("in", false);
            anim.SetBool("clicked", false);
            //tray.isClicked = false;
        }
    }
    public void doneCooking()
    {
        done = true;
        isCooking = false;
    }
    public void moneyBag()
    {
        GameObject par = Instantiate(moneyPlus_particle,transform.parent.gameObject.transform.parent);
        par.transform.position = transform.GetChild(0).position;
        par.transform.localScale = transform.localScale * 0.2f;
        par.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "+"+coins.ToString("##,#");
        par.SetActive(true);
        gm.addMoney(coins);
        anim.SetTrigger("out");
    }
    public void destroying()
    {
        Destroy(gameObject);
    }
}
