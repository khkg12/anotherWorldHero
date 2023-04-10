using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class ItemSelect : MonoBehaviour
{
    public Button FirstBtn;
    public Button SecondBtn;
    public Button ThirdBtn;
    public Button RerollBtn;
    public Button ContinueBtn;

    public List<Item> selectItemList;
    public Image ItemGetUI; // ���������� ������ �������� ��ü�� ����â
    public TextMeshProUGUI ItemGetName;
    public Image ItemGetImage;
    public TextMeshProUGUI ItemGetOption;

    public Image firstItemImage;
    public TextMeshProUGUI firstItemName;
    public TextMeshProUGUI firstItemOption;
    
    public Image SecondItemImage;
    public TextMeshProUGUI SecondItemName;
    public TextMeshProUGUI SecondItemOption;
    
    public Image ThirdItemImage;
    public TextMeshProUGUI ThirdItemName;
    public TextMeshProUGUI ThirdItemOption;    
    

    private void Start()
    {                
        FirstBtn.onClick.AddListener(() => ItemGet(selectItemList[0])); // ù��° ��ư ������ �� ù��° ������ �ɷ�ġ ����
        SecondBtn.onClick.AddListener(() => ItemGet(selectItemList[1]));
        ThirdBtn.onClick.AddListener(() => ItemGet(selectItemList[2]));
        RerollBtn.onClick.AddListener(() => ItemReroll());
        ContinueBtn.onClick.AddListener(() => DialogManager.Instance.NextPage(UiManager.Instance.ItemSelectUI, selectItemList));        
    }

    public void OnEnable() // ���ӿ�����Ʈ�� Ȱ��ȭ�ɶ����� �ʱ�ȭ��Ű�� �ڵ�
    {
        selectItemList = ItemTable.Instance.ItemList;
        selectItemList = selectItemList.OrderBy(i => Random.value).ToList(); // ����Ʈ�� �� �������� ����
        ItemUISet(firstItemImage, firstItemName, firstItemOption, selectItemList[0]); // ù��° ������ ui ����
        ItemUISet(SecondItemImage, SecondItemName, SecondItemOption, selectItemList[1]);
        ItemUISet(ThirdItemImage, ThirdItemName, ThirdItemOption, selectItemList[2]);
    }

    public void ItemGet(Item selectItem) // ������ ��ư Ŭ�� �� ȣ��Ǵ� �Լ� �ɷ�ġ ��ȭ 
    {
        ItemGetOption.text = ""; // ������ �ɼ� �ؽ�ƮUI �ʱ�ȭ
        ItemGetUI.gameObject.SetActive(true);

        PlayerTable.Instance.MaxHp += selectItem.maxHp;
        PlayerTable.Instance.Hp += selectItem.maxHp; // ����hp�� �����ϴ� �ִ�ü�¸�ŭ ���
        PlayerTable.Instance.Hp += selectItem.Hp;
        PlayerTable.Instance.Atk += selectItem.Atk;
        PlayerTable.Instance.Defense += selectItem.Def;
        PlayerTable.Instance.Critical += selectItem.Cri;
        PlayerTable.Instance.Dodge += selectItem.Dod;

        ItemGetName.text = selectItem.name;
        ItemGetImage.sprite = selectItem.sprite;        
        if(selectItem.maxHp > 0)
        {
            ItemGetOption.text += $"�ִ�ü�� : <color=#369341>{PlayerTable.Instance.MaxHp} (+{selectItem.maxHp})</color>" + "\n";
        }
        if (selectItem.Hp > 0)
        {
            ItemGetOption.text += $"����ü�� : <color=#369341>{PlayerTable.Instance.Hp} (+{selectItem.Hp})</color>" + "\n";
        }
        if (selectItem.Atk > 0)
        {
            ItemGetOption.text += $"���ݷ� : <color=#369341>{PlayerTable.Instance.Atk} (+{selectItem.Atk})</color>" + "\n";
        }
        if (selectItem.Def > 0)
        {
            ItemGetOption.text += $"���� : <color=#369341>{PlayerTable.Instance.Defense} (+{selectItem.Def})</color>" + "\n";
        }
        if (selectItem.Cri > 0)
        {
            ItemGetOption.text += $"ġ��Ÿ : <color=#369341>{PlayerTable.Instance.Critical}% (+{selectItem.Cri}%)</color>" + "\n";
        }
        if (selectItem.Dod > 0)
        {
            ItemGetOption.text += $"ȸ���� : <color=#369341>{PlayerTable.Instance.Dodge}% (+{selectItem.Dod}%)</color>" + "\n";
        }        
    }

    public void ItemUISet(Image itemImage, TextMeshProUGUI itemName, TextMeshProUGUI itemOption, Item selectItem) // ������ ui ���� �Լ�
    {
        itemOption.text = ""; // ������ �ɼ� �ʱ�ȭ
        itemImage.sprite = selectItem.sprite;
        itemName.text = selectItem.name;
        for(int i = 0; i < selectItem.option.Length; i++)
        {
            itemOption.text += selectItem.option[i] + "\n";
        }        
    }

    public void ItemReroll()
    {
        firstItemOption.text = "";
        SecondItemOption.text = "";
        ThirdItemOption.text = "";
        selectItemList = selectItemList.OrderBy(i => Random.value).ToList();
        ItemUISet(firstItemImage, firstItemName, firstItemOption, selectItemList[0]); 
        ItemUISet(SecondItemImage, SecondItemName, SecondItemOption, selectItemList[1]);
        ItemUISet(ThirdItemImage, ThirdItemName, ThirdItemOption, selectItemList[2]);
    }       
}
