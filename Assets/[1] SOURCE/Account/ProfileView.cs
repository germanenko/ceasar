using Doozy.Runtime.Signals;
using Doozy.Runtime.UIManager.Animators;
using Doozy.Runtime.UIManager.Containers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfileView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _emailText;
    [SerializeField] private TextMeshProUGUI _roleText;
    [SerializeField] private RawImage _avatar;

    [SerializeField] private UIContainerUIAnimator _profileViewAnimator;

    private void Start()
    {
        LoadInfo();
    }



    public void PickImage()
    {
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            Debug.Log("Image path: " + path);
            if (path != null)
            {
                // Create Texture from selected image
                Texture2D texture = NativeGallery.LoadImageAtPath(path);
                texture.Reinitialize(texture.width, texture.height);

                print(texture.width);
                print(texture.height);
                Texture2D t = new Texture2D(texture.width, texture.height);
                t.SetPixels(texture.GetPixels());
                t.Apply();
                if (texture == null)
                {
                    Debug.Log("Couldn't load texture from " + path);
                    return;
                }
                else
                {
                    _avatar.texture = t;
                }

                // Assign texture to a temporary quad and destroy it after 5 seconds
                //GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
                //quad.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 2.5f;
                //quad.transform.forward = Camera.main.transform.forward;
                //quad.transform.localScale = new Vector3(1f, texture.height / (float)texture.width, 1f);

                //Material material = quad.GetComponent<Renderer>().material;
                //if (!material.shader.isSupported) // happens when Standard shader is not included in the build
                //    material.shader = Shader.Find("Legacy Shaders/Diffuse");

                //material.mainTexture = texture;
                
                //Destroy(quad, 5f);

                // If a procedural texture is not destroyed manually, 
                // it will only be freed after a scene change
                //Destroy(texture, 5f);
            }
        });

        Debug.Log("Permission result: " + permission);
    }



    public void LoadInfo()
    {
        if (AccountManager.Instance == null) return;

        _emailText.text = AccountManager.Instance.ProfileData.email;
        _roleText.text = AccountManager.Instance.ProfileData.role.ToString();

        //_avatar.texture = await ServerConstants.Instance.GetAvatarAsync();
        ServerConstants.Instance.StartCoroutine(ServerConstants.Instance.DownloadTexture((tex) =>
        {
            _avatar.texture = tex;
        }));
    }



    public void SignalListen(Signal signal)
    {
        if(signal.stream.category == "Swipe")
        {
            if(signal.stream.name == "Left")
            {
                _profileViewAnimator.showAnimation.Move.fromDirection = Doozy.Runtime.Reactor.MoveDirection.Right;
                _profileViewAnimator.hideAnimation.Move.toDirection = Doozy.Runtime.Reactor.MoveDirection.Left;
            }
            else if(signal.stream.name == "Right")
            {
                _profileViewAnimator.showAnimation.Move.fromDirection = Doozy.Runtime.Reactor.MoveDirection.Left;
                _profileViewAnimator.hideAnimation.Move.toDirection = Doozy.Runtime.Reactor.MoveDirection.Right;
            }
        }
    }



    public void Deauth()
    {
        AccountManager.Instance.Deauth();
    }
}
