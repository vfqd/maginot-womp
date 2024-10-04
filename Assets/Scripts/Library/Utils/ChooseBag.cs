using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

namespace Library.Utils
{
/** <summary>
    Allows for selecting of an item from a group using weighted random chances
    <example>Example usage:<code>
    
    var chooseBag = new ChooseBag &lt;string&gt;
    (
        (chance: 0.5f, result: "value 1"),
        (chance: 2f, result: "value 2"),
        (chance: 4f, result: "value 3")
    );

    chooseBag.AddElement((chance: 0.25f, result: "value 4"));
    chooseBag.RemoveElement((chance: 4f, result: "value 3"));
    
    print(chooseBag.Choose());
    </code></example>
</summary>
<typeparam name="T"></typeparam>**/

public class ChooseBag<T>
{
    private readonly List<(float chance, T result)> _elements;
    private (float chance, T result)[] _normalizedElements;
    
    public ChooseBag(params (float chance, T result)[] elements)
    {
        _elements = new List<(float chance, T result)>(elements);
        if (elements.Length == 0) return;
        _normalizedElements = NormalizeElements();
    }

    public void AddElement((float chance, T result) element)
    {
        _elements.Add(element);
        _normalizedElements = NormalizeElements();
    }
    
    public void RemoveElement((float chance, T result) element)
    {
        if (_elements.Contains(element)) _elements.Remove(element);
        _normalizedElements = NormalizeElements();
    }

    public T Choose()
    {
        var val = Random.value;

        float rollingSum = 0;
        foreach (var element in _normalizedElements)
        {
            rollingSum += element.chance;
            if (val < rollingSum)
            {
                return element.result;
            }
        }
        throw new ArgumentOutOfRangeException();
    }

    private (float chance, T result)[] NormalizeElements()
    {
        float totalSum = 0;
        foreach (var element in _elements)
        {
            totalSum += element.chance;
        }
        List<(float chance, T result)> normalizedChances = new List<(float chance, T result)>();
        float normalizedSum = 0;
        
        foreach (var bagElement in _elements)
        {
            var normalizedChance = bagElement.chance / totalSum;
            normalizedChances.Add((chance:normalizedChance, result:bagElement.result));
            normalizedSum += normalizedChance;
        }
        Assert.IsTrue(Mathf.Approximately(normalizedSum ,1), $"Sum is {normalizedSum} not equal to 1. {typeof(T)}");
        return normalizedChances.ToArray();
    }
}
}