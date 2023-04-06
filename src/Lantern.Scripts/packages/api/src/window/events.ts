import { Position, Size } from "../models";

export interface ResizedEvent {
  size: Size;
}

export interface MovedEvent {
  position: Position;
}
