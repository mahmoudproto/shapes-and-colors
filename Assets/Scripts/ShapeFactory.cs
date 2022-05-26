using System;
using UnityEngine;
namespace Assets
{
    
    public enum Shapesname
    {
        cube,
        cylinder,
        sphere
    }

    [System.Serializable]
    public class ShapeData
    {
        public Shapesname name_enum;
        public float[] rotation;
        public float[] Color;

        public ShapeData()
        {
            rotation = new float[4] {0,0.25f,0.25f,1};
            Color = new float[4]{ 1, 1, 1, 1 };
        }
    }

    [System.Serializable]
    public class Shape
    {
        public GameObject shapeGameObject;
        public ShapeData shape_data;


        public Shape(GameObject prefab, ShapeData shape_data)
        {
            this.shapeGameObject = prefab;
            this.shape_data = shape_data;
            Draw();
        }

        public void Draw()
        {
            shapeGameObject = GameObject.Instantiate<GameObject>(this.shapeGameObject);
        }

        public void ChangeColor(Color new_color)
        {
            this.shapeGameObject.GetComponent<MeshRenderer>().material.color = new_color;
            // update shape data to be ready to save it
            this.shape_data.Color = new float[] { new_color.r, new_color.g, new_color.b, new_color.a };
        }

        public void Rotate(float rotation_speed)
        {
            float rotX = Input.GetAxis("Mouse X") * rotation_speed * Mathf.Deg2Rad;
            float rotY = Input.GetAxis("Mouse Y") * rotation_speed * Mathf.Deg2Rad;
            Transform shape_transform = this.shapeGameObject.transform;
            shape_transform.RotateAround(shapeGameObject.transform.position, Vector3.up, -rotX);
            shape_transform.RotateAround(shapeGameObject.transform.position, Vector3.right, rotY);
            // update shape data to be ready to save it
            this.shape_data.rotation[0] = shape_transform.rotation.x;
            this.shape_data.rotation[1] = shape_transform.rotation.y;
            this.shape_data.rotation[2] = shape_transform.rotation.z;
            this.shape_data.rotation[3] = shape_transform.rotation.w;
        }

        public void ApplySavedColorAndRotation(float[] color, float[] rotation)
        {
            shapeGameObject.transform.rotation = new Quaternion(rotation[0], rotation[1], rotation[2], rotation[3]);
            this.ChangeColor(new Color(color[0], color[1], color[2], color[3]));
        }

    }

    public class Cylinder : Shape
    {
        public Cylinder(GameObject pre,ShapeData shape_Data):base(pre,shape_Data)
        {
        }
    }

    public class Cube : Shape
    {
        public Cube(GameObject pre, ShapeData shape_Data) : base(pre, shape_Data)
        {
        }
    }

    public class Sphere : Shape
    {
        public Sphere(GameObject pre, ShapeData shape_Data) : base(pre, shape_Data)
        {
        }
    }

    public class ShapeFactory
    {
        public Shape creatshape(GameObject prefab,ShapeData shape_data)
        {
            switch (shape_data.name_enum)
            {
                case Shapesname.cube:
                    return new Cube(prefab,shape_data);
                case Shapesname.cylinder:
                    return new Cylinder(prefab,shape_data);
                case Shapesname.sphere:
                    return new Sphere(prefab,shape_data);
                default:
                    return null;
            }
        }
    }
}
