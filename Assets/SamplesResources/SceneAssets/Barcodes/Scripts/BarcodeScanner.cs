/*===============================================================================
Copyright (c) 2022 PTC Inc. All Rights Reserved.

Vuforia is a trademark of PTC Inc., registered in the United States and other
countries.
===============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class BarcodeScanner : MonoBehaviour
{
    public Text SelectedBarcodeText;
    public RectTransform TwoDReticle;
    public RectTransform OneDReticle;

    public float BarcoudeOutlineThickness = 1.0f;
    public Material BarcodeOutlineMaterial;

    //public BarcodeInstanceBehaviour BarcodeInstanceTemplate;
    
    Mode mMode = Mode.SCAN_2D;
    
    public enum Mode
    {
        SCAN_1D,
        SCAN_2D,
        ALL
    }

    static readonly BarcodeBehaviour.BarcodeType[] OneDTypes =
    {
        BarcodeBehaviour.BarcodeType.UPCA,
        BarcodeBehaviour.BarcodeType.UPCE,
        BarcodeBehaviour.BarcodeType.EAN8,
        BarcodeBehaviour.BarcodeType.EAN13,
        BarcodeBehaviour.BarcodeType.CODE39,
        BarcodeBehaviour.BarcodeType.CODE93,
        BarcodeBehaviour.BarcodeType.CODE128,
        BarcodeBehaviour.BarcodeType.CODABAR,
        BarcodeBehaviour.BarcodeType.ITF
    };

    static readonly BarcodeBehaviour.BarcodeType[] TwoDTypes =
    {
        BarcodeBehaviour.BarcodeType.QRCODE,
        BarcodeBehaviour.BarcodeType.DATAMATRIX,
        BarcodeBehaviour.BarcodeType.AZTEC,
        BarcodeBehaviour.BarcodeType.PDF417,
    };

    BarcodeBehaviour mBarcodeBehaviour;

    readonly HashSet<BarcodeBehaviour> mBarcodeInstances = new HashSet<BarcodeBehaviour>();

    /// <summary>
    /// Called when the script is loaded
    /// </summary>
    void Awake()
    {
        VuforiaBehaviour.Instance.World.OnObserverCreated += OnObserverCreated;
        VuforiaBehaviour.Instance.World.OnObserverDestroyed += OnObserverDestroyed;
    }

    /// <summary>
    /// Called when the script is destroyed
    /// </summary>
    void OnDestroy()
    {
        if (VuforiaBehaviour.Instance != null && 
            VuforiaBehaviour.Instance.World != null)
        {
            VuforiaBehaviour.Instance.World.OnObserverCreated -= OnObserverCreated;
            VuforiaBehaviour.Instance.World.OnObserverDestroyed -= OnObserverDestroyed;
        }
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        var selectedBehaviour = FindClosestBarcode();

        foreach (var behaviour in mBarcodeInstances)
        {
            if (behaviour != selectedBehaviour)
            {
                UnselectBarcode(behaviour);
            }
        }

        SelectBarcode(selectedBehaviour);
    }

    /// <summary>
    /// Find the barcode instance closest to the center of the image that intersects with the search reticle
    /// </summary>
    BarcodeBehaviour FindClosestBarcode()
    {
        var arCamera = VuforiaBehaviour.Instance.GetComponent<Camera>();
        var reticle =  mMode == Mode.SCAN_1D ? OneDReticle : TwoDReticle;
        var canvasRect = reticle.GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        
        // Get the size of the Reticle in normalized Viewport coordinates (0,1)
        var normalizedReticleSize = reticle.rect.size / canvasRect.rect.size ;

        var normalizedReticleRect = new Rect((Vector2.one - normalizedReticleSize) * 0.5f, normalizedReticleSize);

        BarcodeBehaviour selectedBehaviour = null;
        float shortestDistance = float.MaxValue;

        // Select the BarcodeBehaviour that intersects the Search Reticle closest to the center of the image.
        foreach (var behaviour in mBarcodeInstances.Where(behaviour => behaviour.InstanceData != null))
        {
            var verticesViewportSpace = behaviour.InstanceData.OutlineVertices
                .Select(v => arCamera.WorldToViewportPoint(behaviour.transform.TransformPoint(v)))
                .ToList();

            var centerViewportSpace = arCamera.WorldToViewportPoint(behaviour.transform.position);
 
            var bounds = new Bounds(verticesViewportSpace[0], Vector3.zero);

            foreach(var v in verticesViewportSpace) 
                bounds.Encapsulate(v);

            var barcodeRect = new Rect(bounds.min, bounds.size);

            if (!normalizedReticleRect.Overlaps(barcodeRect))
            {
                continue;
            }

            var distance = Vector2.Distance(centerViewportSpace, new Vector2(0.5f, 0.5f));

            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                selectedBehaviour = behaviour;
            }
        }

        return selectedBehaviour;
    }

    /// <summary>
    /// Set the Barcode scanner mode
    /// </summary>
    public void SetBarcodeScannerMode(Mode mode)
    {
        mMode = mode;
        SelectBarcode(null);

        switch (mMode)
        {
            case Mode.SCAN_1D:
                CreateBarcodeBehaviour(OneDTypes);
                OneDReticle.gameObject.SetActive(true);
                TwoDReticle.gameObject.SetActive(false);
                break;
            case Mode.SCAN_2D:
                CreateBarcodeBehaviour(TwoDTypes);
                OneDReticle.gameObject.SetActive(false);
                TwoDReticle.gameObject.SetActive(true);
                break;
            case Mode.ALL:
                CreateBarcodeBehaviour(OneDTypes.Concat(TwoDTypes).ToArray());
                OneDReticle.gameObject.SetActive(false);
                TwoDReticle.gameObject.SetActive(true);
                break;
        }
    }
    
    /// <summary>
    /// Helper function to (re-)create the Barcode Behaviour
    /// </summary>
    void CreateBarcodeBehaviour(BarcodeBehaviour.BarcodeType[] types)
    {
        if (mBarcodeBehaviour != null)
        {
            Destroy(mBarcodeBehaviour.gameObject);
        }

        mBarcodeInstances.Clear();

        // Create a new Barcode Behaviour with the observed types for the requested mode
        mBarcodeBehaviour = VuforiaBehaviour.Instance.ObserverFactory.CreateBarcodeBehaviour(
            new HashSet<BarcodeBehaviour.BarcodeType>(types), 
            true);

        var outline = mBarcodeBehaviour.gameObject.AddComponent<BarcodeOutlineBehaviour>();
        outline.Material = BarcodeOutlineMaterial;
        outline.OutlineThickness = BarcoudeOutlineThickness;
    }

    /// <summary>
    /// Called when a ObserverBehaviour is created
    /// </summary>
    void OnObserverCreated(ObserverBehaviour behaviour)
    {
        if (behaviour is BarcodeBehaviour barcodeBehaviour)
        {
            mBarcodeInstances.Add(barcodeBehaviour);
        }
    }

    /// <summary>
    /// Called when a ObserverBehaviour is destroyed
    /// </summary>
    void OnObserverDestroyed(ObserverBehaviour behaviour)
    {
        if (behaviour is BarcodeBehaviour barcodeBehaviour)
        {
            mBarcodeInstances.Remove(barcodeBehaviour);
        }
    }

    /// <summary>
    /// Unselect the Barcode 
    /// </summary>
    void UnselectBarcode(BarcodeBehaviour behaviour)
    {
        // Deactivate the outline meshes for unselected barcodes
        foreach (var meshRenderer in behaviour.GetComponentsInChildren<MeshRenderer>())
        {
            meshRenderer.enabled = false;
        }
    }

    /// <summary>
    /// Select the Barcode
    /// </summary>
    void SelectBarcode(BarcodeBehaviour behaviour)
    {
        if (behaviour != null && behaviour.InstanceData != null)
        {
            SelectedBarcodeText.text = behaviour.InstanceData.Text;

            // Activate the outline meshes for the selected barcode
            foreach (var meshRenderer in behaviour.GetComponentsInChildren<MeshRenderer>())
            {
                meshRenderer.enabled = true;
            }
        }
        else
        {
            SelectedBarcodeText.text = "";
        }
    }
}
