using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;

public class Experments_Controller : MonoBehaviour
{
    [SerializeField]
    private float rotation_speed;
    private bool isRightMouseDown;
    private Shape current_shape;

    public Text user_messeges;
    public Light light_source;
    public Scrollbar light_slider;
    // the index of the shape inside the array so we can keep track of the shape currently in use
    [SerializeField]
    private int currentshape_index;
    [SerializeField]
    public Shape[] shapes_Array;
    ShapeFactory shapeFactory;

    private void Start()
    {
        shapeFactory = new ShapeFactory();
        //to make a defult shape appear on the screen
        CreatShape(new ShapeData());
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
            isRightMouseDown = true;

        else if (Input.GetMouseButtonUp(1))
            isRightMouseDown = false;

        if (isRightMouseDown)
            RotateShape();
    }

    void CreatShape(ShapeData shape_data)
    {
        //we loop on the array of shapes to creat a shape that matches the shape data given to the function
        for (int i = 0; i < shapes_Array.Length; i++)
        {
            if (shapes_Array[i].shape_data.name_enum== shape_data.name_enum)
            {
                //the index of the shape in the array of shapes
                currentshape_index = i;
                current_shape = shapeFactory.creatshape(shapes_Array[i].shapeGameObject,shape_data);
            }
        }
    }

    public void Save()
    {
        SaveAndLoadSystem.SaveShapeData(current_shape.shape_data);
    }

    public void Load()
    {
        ShapeData LoadedData = SaveAndLoadSystem.LoadShapeData();
        if (LoadedData==null)
        {
            user_messeges.gameObject.SetActive(true);
            user_messeges.text = "you need to save a shape first";
            Invoke("HideMesseges", 2f);
            return;
        }
        RemoveOldShape(current_shape);
        CreatShape(LoadedData);
        current_shape.ApplySavedColorAndRotation(LoadedData.Color, LoadedData.rotation);
    }

    void HideUserMesseges()
    {
        user_messeges.gameObject.SetActive(false);
    } 

    void RemoveOldShape(Shape shape)
    {
        Destroy(shape.shapeGameObject);
    }

    public void GoToNextShape()
    {
        RemoveOldShape(current_shape);
        currentshape_index++;
        //if we reatched the end of the array we make the next shape equal the first one
        if (currentshape_index == shapes_Array.Length)
        {
            currentshape_index = 0;
        }
        //we need to call the constructor of the shape data to set the defult rotation and color of a shape
        //but we still need to maintain the shape name 
        Shapesname temp = shapes_Array[currentshape_index].shape_data.name_enum;
        shapes_Array[currentshape_index].shape_data = new ShapeData();
        shapes_Array[currentshape_index].shape_data.name_enum = temp;
        //
        CreatShape(shapes_Array[currentshape_index].shape_data);
    }

    public void GoToPriveiousShape()
    {
        RemoveOldShape(current_shape);
        currentshape_index--;
        //if we reatched the top of the array we make the previous shape equals to the last one
        if (currentshape_index < 0)
        {
            currentshape_index = shapes_Array.Length - 1;
        }
        //we need to call the constructor of the shape data to set the defult rotation and color of a shape
        //but we still need to maintain the shape name 
        Shapesname temp =shapes_Array[currentshape_index].shape_data.name_enum;
        shapes_Array[currentshape_index].shape_data = new ShapeData();
        shapes_Array[currentshape_index].shape_data.name_enum = temp;
        //
        CreatShape(shapes_Array[currentshape_index].shape_data);

    }

    public void ChangeLightIntinesty()
    {
        light_source.intensity = light_slider.value;
    }

    private void RotateShape()
    {
        current_shape.Rotate(rotation_speed);
    }

    public void ChangeColor(Image ui_image)
    {
        current_shape.ChangeColor(ui_image.color);
    }
}
