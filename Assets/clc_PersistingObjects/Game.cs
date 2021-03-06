﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Game : PersistableObject
{
    public PersistableObject prefab;

    public KeyCode createKey = KeyCode.C;
    public KeyCode newGameKey = KeyCode.N;
    public KeyCode saveKey = KeyCode.S;
    public KeyCode loadKey = KeyCode.L;

    private List<PersistableObject> objects;
    private PersistentStorage storage;

    private string savePath;


    void Awake()
    {
        storage = new PersistentStorage();
        objects = new List<PersistableObject>();
        savePath = Path.Combine(Application.persistentDataPath, "saveFile");
        Debug.Log("Save path: " + savePath);
    }

    public override void Save(GameDataWriter writer)
    {
        writer.Write(objects.Count);
        for (int i = 0; i < objects.Count; i++)
        {
            objects[i].Save(writer);
        }
    }

    public override void Load(GameDataReader reader)
    {
        BeginNewGame();

        int count = reader.ReadInt();
        for (int i = 0; i < count; i++)
        {
            PersistableObject o = Instantiate(prefab);
            o.Load(reader);
            objects.Add(o);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(createKey))
        {
            CreateObject();
        }
        else if (Input.GetKey(newGameKey))
        {
            BeginNewGame();
        }
        else if (Input.GetKeyDown(saveKey))
        {
            storage.Save(this);
        }
        else if (Input.GetKeyDown(loadKey))
        {
            storage.Load(this);
        }
    }

    void CreateObject()
    {
        PersistableObject p = Instantiate(prefab);
        Transform t = p.transform;
        t.localPosition = Random.insideUnitSphere * 5f;
        t.localRotation = Random.rotation;
        t.localScale = Vector3.one * Random.Range(0.1f, 1f);
        objects.Add(p);
    }

    void BeginNewGame() {
        for (int i = 0; i < objects.Count; i++)
        {
            Destroy(objects[i].gameObject);
        }
        objects.Clear();
    }

}