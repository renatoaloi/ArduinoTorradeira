const int LM35 = A1; // Pino Analogico onde vai ser ligado ao pino 2 do LM35
const int REFRESH_RATE = 2000;  //Tempo de atualização entre as leituras em ms
const float CELSIUS_BASE = 0.4887585532746823069403714565; //Base de conversão para Graus Celsius ((5/1023) * 100)

// Potenciometro ligado na porta analogica zero do Arduino (A0)

static volatile unsigned long momentoDisparo = 0;
static volatile unsigned long momentoPrint = 0;

static volatile unsigned int jafoi = 0;
static volatile unsigned int gatilhoDisparo = 7000;

static volatile unsigned int potVal = 0;

const unsigned char portaDimmerAC1 = 5;

void setup()
{ 
  pinMode(portaDimmerAC1, OUTPUT);
  digitalWrite(portaDimmerAC1, LOW);
  
  pinMode(8, OUTPUT);
  digitalWrite(8, HIGH);
  pinMode(7, OUTPUT);
  digitalWrite(7, HIGH);
  
  attachInterrupt(0, zeroCrossInt, CHANGE);
  
  Serial.begin(9600);
  delay(100);
  Serial.println("OK!");
  
  momentoPrint = millis() + 1000;
}

void loop()
{
  potVal = analogRead(0);
  gatilhoDisparo = map(potVal, 0, 1023, 7000, 1000);
  
  
  if (momentoDisparo > micros())
  {
    while(momentoDisparo > micros());
    
    if (readTemperature() <= 70.0)
    {
      jafoi = 0;
      digitalWrite(portaDimmerAC1, HIGH);
    }
    else
    {
      if (!jafoi) 
      {
        Serial.println("DEACTIVATE!");
        jafoi = 1;
        // pausa para estabilizar
        // 2 seg
        delay(2000);
      }
    }
  }
  
  if(momentoPrint < millis())
  {
    Serial.print("T:");
    Serial.println(readTemperature());
    momentoPrint = millis() + 1000;
  }
  
  
}

void zeroCrossInt()
{
  digitalWrite(portaDimmerAC1, LOW);
  momentoDisparo = micros() + gatilhoDisparo;
}

float readTemperature(){
  return (analogRead(LM35) * CELSIUS_BASE); 
}
