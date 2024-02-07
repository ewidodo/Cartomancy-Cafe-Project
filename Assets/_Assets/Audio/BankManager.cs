using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BankManager : MonoBehaviour
{
    // yield return StartCoroutine(LoadBank(bankName));
    public IEnumerator LoadBank(string bankName)
    {
        AKRESULT result = AkSoundEngine.LoadBank(0);

        yield return null;
    }
}
