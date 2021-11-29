using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IReadWriteData
{
    public List<T> ReadList<T>(SerializationTypes serializationType) where T : new();
    public T Read<T>(SerializationTypes serializationType) where T : new();

    public bool Save<T>(List<T> obj, SerializationTypes serializationType);
    public bool Save<T>(T obj, SerializationTypes serializationType);

    public string GetPath(SerializationTypes serializationType);
}
