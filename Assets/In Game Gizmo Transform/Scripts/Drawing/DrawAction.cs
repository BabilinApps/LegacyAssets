using UnityEngine;
using System.Collections;

public static class CurVectors
{
    public static Vector3 uForward = Vector3.zero;
    public static Vector3 rForward = Vector3.zero;
    public static Vector3 fForward = Vector3.zero;

}


public class DrawAction {

 

    private Material lineMaterial; // same as SelectedObjOutline
    public Color circleX, circleY, circleZ, selX, selY, selZ, panY, panX, panZ;
    private Vector3 Dir = Vector3.zero; //The current derections of the Gizmos and object
    public Color LineColor;         // if "UseOwnColor" is true then set the color of the line to this
    public Camera _Cam;
    public bool Global;
    public bool FullcircleGizmo;
    //use this to turn off all gizmos 

   public void NothingSelected()
    {
        circleX = Color.red;
        circleY = Color.green;
        circleZ = Color.blue;
        panY = Color.green;
        panX = Color.red;
        panZ = Color.blue;
        selX = Color.red;
        selY = Color.green;
        selZ = Color.blue;
        lineMaterial = new Material(Shader.Find("Custom/GizmoShader"));
    }

    public void RenderTransformGizmos(Matrix4x4 rotationMatrix) {
        GL.PushMatrix();

        GL.MultMatrix(rotationMatrix);
        lineMaterial.SetPass(0);


        GL.PopMatrix();

    }

    public void RenderGizmos(GameObject Item, float SetSpread, Matrix4x4 rotationMatrix, Vector3 matScal)
    {

        Dir = _Cam.transform.position - Item.transform.position;

        Dir.Normalize();


        GL.PushMatrix();
        rotationMatrix = Item.transform.localToWorldMatrix;
        if(Global)
        rotationMatrix = Matrix4x4.TRS(Item.transform.position, Quaternion.identity, matScal);
        else
        rotationMatrix = Matrix4x4.TRS(Item.transform.position, Item.transform.localRotation, matScal);

        GL.MultMatrix(rotationMatrix);
        lineMaterial.SetPass(0);



        Vector3 camdir = _Cam.transform.position;

        //project Dir on planeXZ
        if (!Global)
        camdir = Item.transform.InverseTransformPoint(_Cam.transform.position);
        camdir = new Vector3(camdir.x + 1, camdir.y, camdir.z + 1);

        Vector3 prjleft = Vector3.Cross((new Vector3(10, 0, 0)), camdir) * 90;
        prjleft.Normalize();
        prjleft *= SetSpread;
        Vector3 prjfwrd = Vector3.Cross(prjleft, new Vector3(10, 0, 0));
        prjfwrd.Normalize();
        prjfwrd *= SetSpread;
      DrawCircle(circleX, prjleft, prjfwrd);
        CurVectors.rForward = prjfwrd;



        prjleft = Vector3.Cross((new Vector3(0, 10, 0)), camdir);
        prjleft.Normalize();
        prjleft *= SetSpread;
        prjfwrd = Vector3.Cross(prjleft, (new Vector3(0, 10, 0)));
        prjfwrd.Normalize();
        prjfwrd *= SetSpread;
        DrawCircle(circleY, prjleft, prjfwrd);
        CurVectors.uForward = prjfwrd;

        prjleft = Vector3.Cross((new Vector3(0, 0, 10)), camdir) * 15;
        prjleft.Normalize();
        prjleft *= SetSpread;
        prjfwrd = Vector3.Cross(prjleft, new Vector3(0, 0, 10)) * 10;
        prjfwrd.Normalize();
        prjfwrd *= SetSpread;
        DrawCircle(circleZ, prjleft, prjfwrd);
        CurVectors.fForward = prjfwrd;





        GL.PopMatrix();

    }

    



    public void DrawCircle(Color col, Vector3 vtx, Vector3 vty)
    {


        GL.Begin(GL.LINES);
        GL.Color(col);
        if (FullcircleGizmo == true)
        {
            for (int i = 0; i < 200; i++)
            {
                if (true)
                {
                    Vector3 vt;
                    vt = vtx * Mathf.Cos((Mathf.PI / 100) * i);
                    vt += vty * Mathf.Sin((Mathf.PI / 100) * i);
                    GL.Vertex3(vt.x, vt.y, vt.z);
                    vt = vtx * Mathf.Cos((Mathf.PI / 100) * (i + 1));
                    vt += vty * Mathf.Sin((Mathf.PI / 100) * (i + 1));

                    GL.Vertex3(vt.x, vt.y, vt.z);
                }
            }
        }
        else
        {
            for (int i = 0; i < 100; i++)
            {
                if (true)
                {
                    Vector3 vt;
                    vt = vtx * Mathf.Cos((Mathf.PI / 100) * i);
                    vt += vty * Mathf.Sin((Mathf.PI / 100) * i);
                    GL.Vertex3(vt.x, vt.y, vt.z);
                    vt = vtx * Mathf.Cos((Mathf.PI / 100) * (i + 1));
                    vt += vty * Mathf.Sin((Mathf.PI / 100) * (i + 1));

                    GL.Vertex3(vt.x, vt.y, vt.z);
                }
            }
        }
        GL.End();
    }


    public void DrawAxis(Vector3 axis, Vector3 vtx, Vector3 vty, float fct, float fct2, Color col)
    {
       
        GL.Begin(GL.LINES);
        GL.Color(col);
        GL.Vertex3(0, 0, 0);

        GL.Vertex(axis);
        GL.End();


        GL.Begin(GL.TRIANGLES);
        GL.Color(col);
        for (int i = 0; i <= 30; i++)
        {

            Vector3 pt;
            pt = vtx * Mathf.Cos(((2 * Mathf.PI) / 10.0f) * i) * fct * 2;
            pt += vty * Mathf.Sin(((2 * Mathf.PI) / 10.0f) * i) * fct * 2;
            pt += axis * fct2;

            GL.Vertex(pt);
            pt = vtx * Mathf.Cos(((2 * Mathf.PI) / 10.0f) * (i + 1)) * fct * 2;
            pt += vty * Mathf.Sin(((2 * Mathf.PI) / 10.0f) * (i + 1)) * fct * 2;
            pt += axis * fct2;

            GL.Vertex(pt);
            GL.Vertex(axis);



        }
        GL.End();
    }



    public void DrawQuad(float size, bool bSelected, Vector3 axisU, Vector3 axisV, Color col)
    {

        Vector3[] pts = new Vector3[4];
        pts[1] = (axisU * size);
        pts[2] = (axisU + axisV) * size;
        pts[3] = (axisV * size);


        GL.Begin(GL.QUADS);
        col.a = 0.15f;
        if (!bSelected)
            GL.Color(col);
        else
            GL.Color(Color.white);
        GL.End();


        GL.Begin(GL.QUADS);
        col.a = 0.15f;
        if (!bSelected)
            GL.Color(col);
        else
            GL.Color(Color.white);
        GL.Vertex(pts[0]);
        GL.Vertex(pts[1]);
        GL.Vertex(pts[1]);
        GL.Vertex(pts[2]);
        GL.Vertex(pts[2]);
        GL.Vertex(pts[3]);
        GL.Vertex(pts[3]);
        GL.Vertex(pts[0]);

        GL.End();

    }


    public void DrawScale(float size, bool bSelected, Vector3 axisU, Vector3 axisV, Color col)
    {

        Vector3[] pts = new Vector3[3];
        pts[1] = (axisU * size);
        pts[2] = (axisV * size);



        GL.Begin(GL.QUADS);
        col.a = 0.15f;
        if (!bSelected)
            GL.Color(col);
        else
            GL.Color(Color.white);
        GL.End();


        GL.Begin(GL.QUADS);
        col.a = 0.15f;
        if (!bSelected)
            GL.Color(col);
        else
            GL.Color(Color.white);
        GL.Vertex(pts[0]);
        GL.Vertex(pts[1]);
        GL.Vertex(pts[2]);
        GL.Vertex(pts[1]);
        GL.Vertex(pts[2]);
        GL.Vertex(pts[1]);
        GL.Vertex(pts[2]);
        GL.Vertex(pts[0]);

        GL.End();

    }

  public  void DrawShpere(Vector3 axis, Vector3 vtx, Vector3 vty, float fct, float fct2, Color col)
    {




        GL.Begin(GL.LINES);
        GL.Color(col);
        GL.Vertex3(0, 0, 0);

        GL.Vertex(axis);
        GL.End();


        GL.Begin(GL.TRIANGLE_STRIP);
        GL.Color(col);
        for (int i = 0; i <= 30; i++)
        {

            Vector3 pt;
            pt = vtx * Mathf.Cos(((5 * Mathf.PI) / 20.0f) * i) * fct;
            pt += vty * Mathf.Sin(((5 * Mathf.PI) / 20.0f) * i) * fct;
            pt += axis * fct2 * 1.2f;
            GL.Vertex(pt);

            pt = vtx * Mathf.Cos(((5 * Mathf.PI) / 20.0f) * (i + 1)) * fct;
            pt += vty * Mathf.Sin(((5 * Mathf.PI) / 20.0f) * (i + 1)) * fct;
            pt += axis * fct2 * 1.2f;

            GL.Vertex(pt);

            pt = vtx * Mathf.Cos(((5 * Mathf.PI) / 20.0f) * i) * fct;
            pt += vty * Mathf.Sin(((5 * Mathf.PI) / 20.0f) * i) * fct;
            pt += axis * fct2;


            GL.Vertex(pt);
            pt = vtx * Mathf.Cos(((5 * Mathf.PI) / 20.0f) * (i + 1)) * fct;
            pt += vty * Mathf.Sin(((5 * Mathf.PI) / 20.0f) * (i + 1)) * fct;
            pt += axis * fct2;


            GL.Vertex(pt);
            GL.Vertex(new Vector3(axis.x, axis.y, axis.z));



        }
        GL.End();
    }





}
