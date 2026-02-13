# Relearncling – README

This README explains how to **install and use the Relearncling VR application** on a **Meta Quest 3** using the provided `relearncling.apk`, and how to **load and run the full project in Unity**.

---

## 1. Using Relearncling on Meta Quest 3 (APK Installation)

### 1.1 Prerequisites

Before installing the application, make sure you have:

* A **Meta Quest 3** headset
* A **USB-C cable** to connect the headset to your computer
* A **PC (Windows/Linux/macOS)**
* A **Meta developer account**
* **Developer Mode enabled** on the Quest 3
* One of the following tools installed:

  * **SideQuest** (recommended for simplicity)
  * **ADB (Android Debug Bridge)**

---

### 1.2 Enable Developer Mode on Meta Quest 3

1. Create or log in to a Meta developer account.
2. Open the **Meta Quest mobile app** on your phone.
3. Go to **Menu → Devices → Quest 3**.
4. Enable **Developer Mode**.
5. Restart your headset.

---

### 1.3 Install the APK using SideQuest (Recommended)

1. Download and install **SideQuest** on your computer.
2. Connect your Meta Quest 3 to your computer using a USB-C cable.
3. Put on the headset and **allow USB debugging** when prompted.
4. Open SideQuest — a green indicator should confirm the connection.
5. Drag and drop `relearncling.apk` into SideQuest **or** click *Install APK from file*.
6. Wait for the installation to complete.

---

### 1.4 Install the APK using ADB (Alternative)

If you prefer the command line:

```bash
adb install relearncling.apk
```

Make sure your headset is detected:

```bash
adb devices
```

---

### 1.5 Launching the Application

1. Put on your Meta Quest 3 headset.
2. Go to **Library → Unknown Sources**.
3. Select **Relearncling**.
4. The VR experience will start.

---

## 2. How to Use Relearncling in VR

* Wear the headset and use **Quest controllers** to interact.
* Grab virtual trash objects and place them in the correct recycling bins.
* The game provides **visual feedback and scoring** to help users learn proper recycling practices.
* At the end of a session, a **results screen** displays performance and accuracy.

---

## 3. Loading the Full Project in Unity

### 3.1 Prerequisites

To open and build the project in Unity, you need:

* **Unity Hub**
* **Unity Editor (recommended LTS version, e.g. 2022 LTS)**
* **Android Build Support**, including:

  * Android SDK
  * Android NDK
  * OpenJDK
* A Meta Quest–compatible PC

---

### 3.2 Opening the Project

1. Launch **Unity Hub**.
2. Click **Open → Add project from disk**.
3. Select the root folder of the Relearncling Unity project.
4. Open the project with the correct Unity version.

---

### 3.3 Unity Project Configuration

Ensure the following settings:

#### Platform

* **File → Build Settings**
* Platform: **Android**
* Click **Switch Platform**

#### Player Settings

* **Minimum API Level**: Android 10 or higher
* **Target Device**: Meta Quest
* **Scripting Backend**: IL2CPP
* **ARM64** enabled

#### XR Configuration

* XR Plug-in Management enabled
* **OpenXR** selected
* Meta Quest support activated

---

### 3.4 Running on Meta Quest 3 from Unity

1. Connect the Quest 3 to your computer via USB-C.
2. Allow USB debugging inside the headset.
3. In Unity:

   * Click **Build and Run**
4. The application will be installed and launched automatically.

---


## 4. Troubleshooting

* **App not visible**: Check *Unknown Sources*
* **Black screen**: Verify XR and OpenXR settings
* **APK install fails**: Ensure ARM64 and correct SDK versions
* **Controller issues**: Check Input System and XR bindings

---

## 5. Authors

* Durel Enzo
* Laurencot Mathieu

Project developed as part of **CS 5970 – Virtual Reality**.

---

## 6. License

This project is intended for academic and educational use.
