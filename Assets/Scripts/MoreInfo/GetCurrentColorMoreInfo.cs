using UnityEngine;
using UnityEngine.UI;

public class GetCurrentColorMoreInfo : MonoBehaviour
{

    [SerializeField]
    public Slider red;
    [SerializeField]
    public Slider green;
    [SerializeField]
    public Slider blue;

    void Start() {
        setColor();
    }

    public void setColor(){
        transform.GetComponent<Image>().color = new Color(red.value, green.value, blue.value);
    }
}