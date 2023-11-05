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
        string path = rootPath + "/" + image.name + ".PNG";

        //���µ�����Դ
        TextureImporter texImp = AssetImporter.GetAtPath(path) as TextureImporter;
        AssetDatabase.CreateFolder(rootPath, image.name);

        //���ÿɶ�
        texImp.isReadable = true;
        //���µ�����ԴӦ���趨
        AssetDatabase.ImportAsset(path);

        //����Сͼ��
        foreach (SpriteMetaData metaData in texImp.spritesheet)
        {
            var width = (int)metaData.rect.width;
            var height = (int)metaData.rect.height;
            Texture2D smallImage = new Texture2D(width, height);

            for (int y = (int)metaData.rect.y; y < metaData.rect.y + metaData.rect.height; y++) 
            {
                for (int x = (int)metaData.rect.x; x < metaData.rect.x + metaData.rect.width; x++)
                {
                    //��һͨ������������ôSetPixel��ֵ����ͼƬ����Ĵ�С�ˣ�width,height)
                    smallImage.SetPixel(x-(int)metaData.rect.x, y-(int)metaData.rect.y, image.GetPixel(x, y));
                    Debug.Log("Still Doing");
                }
            }

            //ת������
            //if (smallImage.format != TextureFormat.ARGB32 && smallImage.format != TextureFormat.RGB24)
            //{
            //    Texture2D newTexture = new Texture2D(smallImage.width, smallImage.height);
            //    newTexture.SetPixels(smallImage.GetPixels(0), 0);
            //    smallImage = newTexture;
            //}
            var pngData = smallImage.EncodeToPNG();
            File.WriteAllBytes(rootPath + "/" + image.name + "/" + metaData.name + ".PNG", pngData);
        }
        Debug.Log("Trans pics done");
    }
}

