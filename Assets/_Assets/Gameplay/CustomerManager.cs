using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : Singleton<CustomerManager>
{
    public List<GameObject> customers = new();
    [SerializeField] private Transform customerParent;

    [SerializeField] private float customerLeaveDelay;
    [SerializeField] private float customerArriveDelay;

    private void Start()
    {
        SpawnNextCustomer();
    }

    public void DespawnCurrentCustomer()
    {
        if (Barista.Instance.currentCustomer == null)
        {
            return;
        }
        Barista.Instance.currentCustomer.Despawn();
    }

    public void SpawnNextCustomer()
    {
        if (customers.Count <= 0)
        {
            Debug.Log("All customers have been given drinks!");
            // access sceneloader.cs and load next day/credits
            SceneLoader.Instance.LoadScene("Credits");
            return;
        }

        GameObject customer = Instantiate(customers[0], customerParent);
        Barista.Instance.currentCustomer = customer.GetComponent<Customer>();
        Barista.Instance.currentCustomer.Spawn();
        FortuneDisplay.Instance.GenerateFortuneRegionDisplay(Barista.Instance.currentCustomer);

        customers.RemoveAt(0);
    }

    public void SwapCustomers()
    {
        StartCoroutine(SwapCustomersRoutine());
    }

    private IEnumerator SwapCustomersRoutine()
    {
        yield return new WaitForSeconds(customerLeaveDelay);
        DespawnCurrentCustomer();
        yield return new WaitForSeconds(customerArriveDelay);
        SpawnNextCustomer();
        yield return null;
    }
}
