import { R as Rectangle } from '../models-d1c109e2.js';

declare function getAll(): Promise<Screen[]>;
declare function getCurrent(): Promise<Screen | null>;
interface Screen {
    scaleFactor: number;
    bounds: Rectangle;
    workingArea: Rectangle;
    primary: boolean;
}

export { Screen, getAll, getCurrent };
