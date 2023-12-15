using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ApplicationVersionUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI versionText;
    
    // TODO: Separate this biz logic into a business logic class
    [SerializeField] private string developmentPhaseSuffix;
    [SerializeField] private int developmentPhaseNumber;
    
    // Start is called before the first frame update
    void Start()
    {
        versionText.text = $"PWAF {developmentPhaseSuffix} {developmentPhaseNumber} ({Application.version})";
    }
}
