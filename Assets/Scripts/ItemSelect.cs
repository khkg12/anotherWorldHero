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
    public Image ItemGetUI; // 최종적으로 선택한 아이템의 구체적 정보창
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
        FirstBtn.onClick.AddListener(() => ItemGet(selectItemList[0])); // 첫번째 버튼 눌렀을 때 첫번째 아이템 능력치 제공
        SecondBtn.onClick.AddListener(() => ItemGet(selectItemList[1]));
        ThirdBtn.onClick.AddListener(() => ItemGet(selectItemList[2]));
        RerollBtn.onClick.AddListener(() => ItemReroll());
        ContinueBtn.onClick.AddListener(() => DialogManager.Instance.NextPage(UiManager.Instance.ItemSelectUI, selectItemList));        
    }

    public void OnEnable() // 게임오브젝트가 활성화될때마다 초기화시키는 코드
    {
        selectItemList = ItemTable.Instance.ItemList;
        selectItemList = selectItemList.OrderBy(i => Random.value).ToList(); // 리스트의 값 랜덤으로 정렬
        ItemUISet(firstItemImage, firstItemName, firstItemOption, selectItemList[0]); // 첫번째 아이템 ui 셋팅
        ItemUISet(SecondItemImage, SecondItemName, SecondItemOption, selectItemList[1]);
        ItemUISet(ThirdItemImage, ThirdItemName, ThirdItemOption, selectItemList[2]);
    }

    public void ItemGet(Item selectItem) // 아이템 버튼 클릭 시 호출되는 함수 능력치 강화 
    {
        ItemGetOption.text = ""; // 아이템 옵션 텍스트UI 초기화
        ItemGetUI.gameObject.SetActive(true);

        PlayerTable.Instance.MaxHp += selectItem.maxHp;
        PlayerTable.Instance.Hp += selectItem.maxHp; // 잔존hp도 증가하는 최대체력만큼 상승
        PlayerTable.Instance.Hp += selectItem.Hp;
        PlayerTable.Instance.Atk += selectItem.Atk;
        PlayerTable.Instance.Defense += selectItem.Def;
        PlayerTable.Instance.Critical += selectItem.Cri;
        PlayerTable.Instance.Dodge += selectItem.Dod;

        ItemGetName.text = selectItem.name;
        ItemGetImage.sprite = selectItem.sprite;        
        if(selectItem.maxHp > 0)
        {
            ItemGetOption.text += $"최대체력 : <color=#369341>{PlayerTable.Instance.MaxHp} (+{selectItem.maxHp})</color>" + "\n";
        }
        if (selectItem.Hp > 0)
        {
            ItemGetOption.text += $"현재체력 : <color=#369341>{PlayerTable.Instance.Hp} (+{selectItem.Hp})</color>" + "\n";
        }
        if (selectItem.Atk > 0)
        {
            ItemGetOption.text += $"공격력 : <color=#369341>{PlayerTable.Instance.Atk} (+{selectItem.Atk})</color>" + "\n";
        }
        if (selectItem.Def > 0)
        {
            ItemGetOption.text += $"방어력 : <color=#369341>{PlayerTable.Instance.Defense} (+{selectItem.Def})</color>" + "\n";
        }
        if (selectItem.Cri > 0)
        {
            ItemGetOption.text += $"치명타 : <color=#369341>{PlayerTable.Instance.Critical}% (+{selectItem.Cri}%)</color>" + "\n";
        }
        if (selectItem.Dod > 0)
        {
            ItemGetOption.text += $"회피율 : <color=#369341>{PlayerTable.Instance.Dodge}% (+{selectItem.Dod}%)</color>" + "\n";
        }        
    }

    public void ItemUISet(Image itemImage, TextMeshProUGUI itemName, TextMeshProUGUI itemOption, Item selectItem) // 아이템 ui 셋팅 함수
    {
        itemOption.text = ""; // 아이템 옵션 초기화
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
