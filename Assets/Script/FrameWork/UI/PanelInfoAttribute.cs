using System;


[AttributeUsage(AttributeTargets.Class)]
public class PanelInfoAttribute : Attribute
{
    private UILayer layer;
    private UILife life;
    private bool releaseAssetWhenDestory;
    private string path;
    private string name;

    public UILayer Layer { get => layer; }
    public UILife Life { get => life; }
    public bool ReleaseAssetWhenDestory {  get => releaseAssetWhenDestory; }
    public string Path {  get => path; }
    public string Name { get => name; }

    public PanelInfoAttribute(UILayer _layer, UILife _life, string _path = "", bool _releaseAssetWhenDestory = false)
    {
        layer = _layer;
        life = _life;
        path = _path;
        releaseAssetWhenDestory= _releaseAssetWhenDestory;
        

    }

    public void SetName(string _name,string _path)
    {
        name = _name;

        if(string.IsNullOrEmpty(path))
        {
            this.path = _path;
        }

    }



}
