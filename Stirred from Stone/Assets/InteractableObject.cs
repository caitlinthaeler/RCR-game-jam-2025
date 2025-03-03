using UnityEngine;

public interface IInteractable
{
    string Name { get; }
    void Interact();
}

public interface ICollectable : IInteractable
{
    void Pickup();
}