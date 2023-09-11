using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public static class SpriteSheetPackerImport
{
    [MenuItem("CustomTools/SpriteSheet/ProcessToSprite")]
    static void ProcessToSprite() 
    {
        Texture2D image = Selection.activeObject as Texture2D;//��ȡ��ǰHierarchy����Project�ļ�����ѡ��Ķ���Object
        string rootPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(image));
        Debug.Log(rootPath);
        string path = rootPath + "/" + image.name + ".PNG";

        //���µ�����Դ
        TextureImporter texImp = AssetImporter.GetAtPath(path) as TextureImporter;
        AssetDatabase.CreateFolder(rootPath, image.name);

        //���ÿɶ�
        texImp.isReadable = true;


        //����Сͼ��
        foreach (SpriteMetaData metaData in texImp.spritesheet) 
        {
            var width = (int)metaData.rect.width;
            var height = (int)metaData.rect.height;
            Texture2D smallImage = new Texture2D(width, height);

            for (int y = (int)metaData.rect.y; y < metaData.rect.y + metaData.rect.height; y++)
            {
                for (int x = (int)metaData.rect.x;  x < metaData.rect.x + metaData.rect.width;  x++)
                {
                    smallImage.SetPixel(x,y,image.GetPixel(x,y));
                }
            }
            
            //ת������

            var pngData = smallImage.EncodeToPNG();
            File.WriteAllBytes(rootPath + "/" + image.name + "/" + metaData.name + ".PNG", pngData);


        }
    }
}

