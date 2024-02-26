using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : Singleton<CustomerManager>
{
    [Header("Customer Logic")]
    public List<GameObject> customers = new();
    [SerializeField] private Transform customerParent;
    public int customersPerDay;
    private int todaysCustomerCount;

    [Header("Timing")]
    [SerializeField] private float customerLeaveDelay;
    [SerializeField] private float customerArriveDelay;

    private void Start()
    {
        SceneLoader.Instance.sceneTransitionManager.transitionComplete.AddListener(SpawnNextCustomer);
    }

    private void OnDestroy()
    {
        SceneLoader.Instance.sceneTransitionManager.transitionComplete.RemoveListener(SpawnNextCustomer);
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
        ++todaysCustomerCount;

        if (todaysCustomerCount > customersPerDay)
        {
            Debug.Log("All of today's customers have been given drinks!");
            // access sceneloader.cs and load next day/credits
            SceneLoader.Instance.LoadScene("Yelp");
            return;
        }

        int customerIndex = (SceneLoader.Instance.dayNumber - 1) * customersPerDay;
        // If overflowed, start repeating customers
        while (customerIndex >= customers.Count)
        {
            customerIndex -= customersPerDay;
        }

        GameObject customer = Instantiate(customers[customerIndex], customerParent);
        Barista.Instance.currentCustomer = customer.GetComponent<Customer>();
        StartCoroutine(Barista.Instance.currentCustomer.Spawn());
        FortuneDisplay.Instance.GenerateFortuneRegionDisplay(Barista.Instance.currentCustomer);

        customers.RemoveAt(customerIndex);
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
