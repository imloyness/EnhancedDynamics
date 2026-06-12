#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Users\Loyness\Documents\C# Projs\EnhancedDynamics\EnhancedDynamics\obj\Debug\netstandard2.0\publicized\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 9.1.0.7988
#endregion

using System.Reflection;
using BepInEx.AssemblyPublicizer;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public PlayerManager pm;

    public CharacterController cc;

    [SerializeField]
    [OriginalAttributes(FieldAttributes.Private)]
    public Entity entity;

    public ActivityModifier am;

    public AnalogInputData movementAnalogData;

    public AnalogInputData cameraAnalogData;

    [OriginalAttributes(FieldAttributes.Private)]
    public Vector3 _position;

    [OriginalAttributes(FieldAttributes.Private)]
    public Vector3 _rotation;

    [OriginalAttributes(FieldAttributes.Private)]
    public Vector3 previousPosition;

    [OriginalAttributes(FieldAttributes.Private)]
    public Vector3 _moveX;

    [OriginalAttributes(FieldAttributes.Private)]
    public Vector3 _moveZ;

    [OriginalAttributes(FieldAttributes.Private)]
    public Vector2 _absoluteVector;

    [OriginalAttributes(FieldAttributes.Private)]
    public Vector2 _deltaVector;

    public float walkSpeed;

    public float runSpeed;

    public float stamina;

    public float staminaDrop;

    public float staminaRise;

    public float staminaMax;

    public float height;

    public float realVelocity;

    public float frameVelocity;

    [OriginalAttributes(FieldAttributes.Private)]
    public float sensitivity;

    [OriginalAttributes(FieldAttributes.Private)]
    public bool running;

    public Entity Entity => entity;

    public float StaminaMax => staminaMax + staminaMax / 4f * (float)Singleton<StickerManager>.Instance.StickerValue(Sticker.Stamina);

    public float RealVelocity => realVelocity;

    [OriginalAttributes(MethodAttributes.Private)]
    public void Start()
    {
        stamina = StaminaMax;
        entity.Initialize(pm.ec, base.transform.position);
    }

    [OriginalAttributes(MethodAttributes.Private)]
    public void Update()
    {
        realVelocity = (base.transform.position - previousPosition).magnitude / Time.deltaTime;
        frameVelocity = (base.transform.position - previousPosition).magnitude;
        previousPosition = base.transform.position;
        if (!Singleton<PlayerFileManager>.Instance.authenticMode)
        {
            running = Singleton<InputManager>.Instance.GetDigitalInput("Run", onDown: false);
        }
        else
        {
            running = Singleton<CoreGameManager>.Instance.authenticScreen.LeverOn;
        }

        MouseMove();
        PlayerMove();
    }

    [OriginalAttributes(MethodAttributes.Private)]
    public void MouseMove()
    {
        Quaternion rotation = base.transform.rotation;
        float num = 0f;
        int num2 = 1;
        if (pm.reversed)
        {
            num2 = -1;
        }

        if (!Singleton<PlayerFileManager>.Instance.authenticMode)
        {
            Singleton<InputManager>.Instance.GetAnalogInput(cameraAnalogData, out _absoluteVector, out _deltaVector, 0.1f);
        }
        else
        {
            Singleton<InputManager>.Instance.GetAnalogInput(movementAnalogData, out _absoluteVector, out _deltaVector, 0.1f);
            _deltaVector.x = 0f;
            _deltaVector.y = 0f;
        }

        num = _deltaVector.x * (float)num2 * Singleton<PlayerFileManager>.Instance.mouseCameraSensitivity + _absoluteVector.x * Time.deltaTime * Singleton<PlayerFileManager>.Instance.controllerCameraSensitivity * (float)num2;
        _rotation = Vector3.zero;
        _rotation.y = num * Time.timeScale;
        if (pm.ec.PlayerTimeScale > 0f)
        {
            rotation.eulerAngles += _rotation;
        }

        base.transform.rotation = rotation;
    }

    [OriginalAttributes(MethodAttributes.Private)]
    public void PlayerMove()
    {
        _position = base.transform.position;
        _position.y = height;
        base.transform.position = _position;
        float num = walkSpeed;
        if ((stamina > 0f) & running)
        {
            num = runSpeed;
        }

        int num2 = 1;
        if (pm.reversed)
        {
            num2 = -1;
        }

        if (!Entity.InteractionDisabled)
        {
            Singleton<InputManager>.Instance.GetAnalogInput(movementAnalogData, out _absoluteVector, out _deltaVector);
            _moveX = base.transform.right * _absoluteVector.x * num2;
            _moveZ = base.transform.forward * _absoluteVector.y;
            if (Singleton<PlayerFileManager>.Instance.authenticMode)
            {
                _moveX = Vector3.zero;
            }
        }
        else
        {
            _moveX = Vector3.zero;
            _moveZ = Vector3.zero;
        }

        if (Singleton<PlayerFileManager>.Instance.analogMovement & (num == walkSpeed))
        {
            sensitivity = Mathf.Clamp((_moveX + _moveZ).magnitude, 0f, 1f);
        }
        else
        {
            sensitivity = 1f;
        }

        entity.UpdateInternalMovement((_moveX + _moveZ).normalized * num * sensitivity * pm.PlayerTimeScale);
        StaminaUpdate(((_moveX + _moveZ).normalized * num * sensitivity).magnitude);
    }

    public void StaminaUpdate(float unmodifiedSpeed)
    {
        if (entity.InternalMovement.magnitude > 0f)
        {
            if (running && entity.RelativeToForcedVelocity.magnitude > walkSpeed * Time.deltaTime * pm.PlayerTimeScale * entity.ExternalActivity.Multiplier)
            {
                stamina = Mathf.Max(stamina - staminaDrop * Time.deltaTime * pm.PlayerTimeScale, 0f);
                if (stamina > 0f)
                {
                    pm.RuleBreak("Running", 0.1f);
                }
            }
        }
        else if (stamina < StaminaMax)
        {
            stamina += staminaRise * Time.deltaTime * pm.PlayerTimeScale;
            if (stamina > StaminaMax)
            {
                stamina = StaminaMax;
            }
        }

        Singleton<CoreGameManager>.Instance.GetHud(pm.playerNumber).SetStaminaValue(stamina / StaminaMax);
    }

    public void AddStamina(float value, bool limited)
    {
        if (limited)
        {
            if (stamina + value > StaminaMax)
            {
                stamina += Mathf.Max(0f, StaminaMax - stamina);
            }
            else
            {
                stamina += value;
            }
        }
        else
        {
            stamina += value;
        }
    }
}
#if false // Decompilation log
'129' items in cache
------------------
Resolve: 'UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Found single assembly: 'UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Load from: 'C:\Users\Loyness\Documents\C# Projs\EnhancedDynamics\Managed\UnityEngine.CoreModule.dll'
------------------
Resolve: 'UnityEngine.PhysicsModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Could not find by name: 'UnityEngine.PhysicsModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
------------------
Resolve: 'netstandard, Version=2.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'
Found single assembly: 'netstandard, Version=2.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'
Load from: 'C:\Users\Loyness\.nuget\packages\netstandard.library\2.0.3\build\netstandard2.0\ref\netstandard.dll'
------------------
Resolve: 'UnityEngine.AnimationModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Could not find by name: 'UnityEngine.AnimationModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
------------------
Resolve: 'UnityEngine.UIModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Could not find by name: 'UnityEngine.UIModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
------------------
Resolve: 'UnityEngine.AudioModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Could not find by name: 'UnityEngine.AudioModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
------------------
Resolve: 'com.rlabrecque.steamworks.net, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Could not find by name: 'com.rlabrecque.steamworks.net, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
------------------
Resolve: 'Unity.RenderPipelines.Universal.Runtime, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Could not find by name: 'Unity.RenderPipelines.Universal.Runtime, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
------------------
Resolve: 'Rewired_Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'
Found single assembly: 'Rewired_Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'
Load from: 'C:\Users\Loyness\Documents\C# Projs\EnhancedDynamics\Managed\Rewired_Core.dll'
------------------
Resolve: 'UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'
Found single assembly: 'UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'
Load from: 'C:\Users\Loyness\Documents\C# Projs\EnhancedDynamics\Managed\UnityEngine.UI.dll'
------------------
Resolve: 'UnityEngine.InputLegacyModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Could not find by name: 'UnityEngine.InputLegacyModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
------------------
Resolve: 'Unity.TextMeshPro, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Found single assembly: 'Unity.TextMeshPro, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Load from: 'C:\Users\Loyness\Documents\C# Projs\EnhancedDynamics\Managed\Unity.TextMeshPro.dll'
------------------
Resolve: 'UnityEngine.TextCoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Could not find by name: 'UnityEngine.TextCoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
------------------
Resolve: 'UnityEngine.AIModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Could not find by name: 'UnityEngine.AIModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
------------------
Resolve: 'UnityEngine.ParticleSystemModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Could not find by name: 'UnityEngine.ParticleSystemModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
------------------
Resolve: 'NavMeshComponents, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Could not find by name: 'NavMeshComponents, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
------------------
Resolve: 'UnityEngine.IMGUIModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Could not find by name: 'UnityEngine.IMGUIModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
------------------
Resolve: 'UnityEngine.UnityWebRequestModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Could not find by name: 'UnityEngine.UnityWebRequestModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
------------------
Resolve: 'UnityEngine.JSONSerializeModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Could not find by name: 'UnityEngine.JSONSerializeModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
------------------
Resolve: 'Rewired_Windows, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'
Could not find by name: 'Rewired_Windows, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'
------------------
Resolve: 'UnityEngine.TextRenderingModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Found single assembly: 'UnityEngine.TextRenderingModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Load from: 'C:\Users\Loyness\Documents\C# Projs\EnhancedDynamics\Managed\UnityEngine.TextRenderingModule.dll'
------------------
Resolve: 'System.Runtime.InteropServices, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null'
Found single assembly: 'System.Runtime.InteropServices, Version=4.1.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '2.0.0.0', Got: '4.1.2.0'
Load from: 'C:\Users\Loyness\.nuget\packages\netstandard.library\2.0.3\build\netstandard2.0\ref\System.Runtime.InteropServices.dll'
------------------
Resolve: 'System.Runtime.CompilerServices.Unsafe, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null'
Could not find by name: 'System.Runtime.CompilerServices.Unsafe, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null'
#endif
