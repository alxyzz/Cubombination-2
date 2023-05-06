#define Echo_EnterPin 5 // Echo input pin
#define Trigger_ExitPin 6 // Trigger output pin

int maximumRange = 300;
int minimumRange = 2;
long distance;
long duration;

int sensorValue1;
int sensorValue2;

void setup() {
  Serial.begin (9600);
  pinMode(Trigger_ExitPin, OUTPUT);
  delayMicroseconds (2);
  pinMode(Echo_EnterPin, INPUT);
  delayMicroseconds (10);
}

void loop() {
  digitalWrite (Trigger_ExitPin, LOW);
  delayMicroseconds (2);
  digitalWrite (Trigger_ExitPin, HIGH);
  delayMicroseconds (10);
  digitalWrite (Trigger_ExitPin, LOW);

  duration = pulseIn (Echo_EnterPin, HIGH);

  distance = (duration / 2) / 29.1;

  Serial.print("Distance:f:");
  Serial.print(distance);
  Serial.print(",Duration:f:");
  Serial.print(duration);
  Serial.println();
  // The calculated distance is output in the serial output
  //Serial.println (sensorData);
  delay (200);
}
