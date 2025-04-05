using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
//Com base neste Script - vamos melhorar nosso carro e deixa-lo mais tunano 😜
//CRIAR UM CARRO COM MOTOR 4 POR 4
//CRIAR UM CARRO COM FREIO NAS QUATRO RODAS
//CRIAR UM JIP E UM CAMINHÃO.😱 
//CRIE UM FREIO DE MÃO .
//ACENDER E APAGAR A LUZ DO CARRO

public class CarController : MonoBehaviour
{    
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    private float horizontalInput;
    private float verticalInput;
    private float currentSteerAngle;
    private float currentBreakForce;
    private bool isBreaking;
    private bool isLightining = true;


    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider frontRighttWheelCollider;
    [SerializeField] private WheelCollider RearLeftWheelCollider;
    [SerializeField] private WheelCollider RearRightWheelCollider;

    
    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform frontRighttWheelTransform;
    [SerializeField] private Transform RearLeftWheelTransformr;
    [SerializeField] private Transform RearRightWheelTransform;


    [SerializeField] private float motorForce;
    [SerializeField] private float breakForce;
    [SerializeField] private float maxSteeringAngle;

    [SerializeField] private Light frontLeftLight;
    [SerializeField] private Light frontRightLight;
    [SerializeField] private Light backLeftLight;
    [SerializeField] private Light backRightLight;

    private void FixedUpdate() 
    {
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
        RestartPosition(); 
        Lightining();


    }

    private void Lightining()
    {
       if(Input.GetKey(KeyCode.L))
       {
           if(isLightining){
            Debug.Log("apaga");
             frontLeftLight.enabled = false;
             frontRightLight.enabled = false;
             backLeftLight.enabled = false;
             backRightLight.enabled = false;
             isLightining = false;
           }else{
            Debug.Log("acende");
            frontLeftLight.enabled = true;
            frontRightLight.enabled = true;
            backLeftLight.enabled = true;
            backRightLight.enabled = true;
            isLightining = true;
           }
       }
    }

    private void RestartPosition()
    {
       if(Input.GetKey("r"))
       {
           Debug.Log("resta");
           transform.position = new Vector3(3f, transform.position.y, transform.position.z);
           transform.rotation = Quaternion.Euler(0f, 0f, 0f);
       }
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxis(HORIZONTAL);
        verticalInput = Input.GetAxis(VERTICAL);
        isBreaking = Input.GetKey(KeyCode.Space);
    }

    private void HandleMotor()
    {
        frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
        frontRighttWheelCollider.motorTorque = verticalInput * motorForce;
        RearLeftWheelCollider.motorTorque = verticalInput * motorForce;
        RearRightWheelCollider.motorTorque = verticalInput * motorForce;
        currentBreakForce = isBreaking ? breakForce : 0f;
        ApplyBreaking();   
    }

    private void ApplyBreaking()
    {
        frontRighttWheelCollider.brakeTorque = currentBreakForce;
        frontLeftWheelCollider.brakeTorque = currentBreakForce; 
        RearLeftWheelCollider.brakeTorque = currentBreakForce;
        RearRightWheelCollider.brakeTorque = currentBreakForce;
    }

    private void HandleSteering()
    {
        currentSteerAngle = maxSteeringAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRighttWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels()
    {
       UpdateSingleWheelCollider(frontLeftWheelCollider, frontLeftWheelTransform);
       UpdateSingleWheelCollider(frontRighttWheelCollider, frontRighttWheelTransform);
       UpdateSingleWheelCollider(RearRightWheelCollider, RearRightWheelTransform);
       UpdateSingleWheelCollider(RearLeftWheelCollider, RearLeftWheelTransformr);
       
    }

    private void UpdateSingleWheelCollider( WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos,  out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }

}
