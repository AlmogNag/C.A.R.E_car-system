export type Participant = {
  fullName: string
  phone: string
  password: string
  cameraId: string
}

export type AlertPayload = {
  isAlert?: boolean
  timestamp?: string
  alertSource?: 'PIR' | 'CAMERA' | 'BOTH' | string
  motionDetected?: boolean
  humanDetected?: boolean
  imageUrl?: string
}

export type AlertHistoryItem = AlertPayload & {
  id: string
}

export type AuthSession = {
  fullName: string
  phone: string
  cameraId: string
}
