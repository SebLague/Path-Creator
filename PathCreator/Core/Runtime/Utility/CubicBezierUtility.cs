using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PathCreation.Utility
{

    /// Collection of functions related to cubic bezier curves
    /// (a curve with a start and end 'anchor' point, and two 'control' points to define the shape of the curve between the anchors)
    public static class CubicBezierUtility
    {
        /// Returns point at time 't' (between 0 and 1) along bezier curve defined by 4 points (anchor_1, control_1, control_2, anchor_2)
        public static Vector3 EvaluateCurve(Vector3[] points, float t)
        {
            Debug.Assert(points.Length == 4, "Incorrect number of points supplied to cubic bezier function (expected 4)");
            if (points.Length >= 4)
            {
                return EvaluateCurve(points[0], points[1], points[2], points[3], t);
            }
            return Vector3.zero;
        }

        /// Returns point at time 't' (between 0 and 1)  along bezier curve defined by 4 points (anchor_1, control_1, control_2, anchor_2)
        public static Vector3 EvaluateCurve(Vector3 a1, Vector3 c1, Vector3 c2, Vector3 a2, float t)
        {
            t = t < 0f ? 0f : t > 1f ? 1f : t;

            float s1 = (1 - t) * (1 - t) * (1 - t);
            float s2 = 3 * (1 - t) * (1 - t) * t;
            float s3 = 3 * (1 - t) * t * t;
            float s4 = t * t * t;

            float x = s1 * a1.x + s2 * c1.x + s3 * c2.x + s4 * a2.x;
            float y = s1 * a1.y + s2 * c1.y + s3 * c2.y + s4 * a2.y;
            float z = s1 * a1.z + s2 * c1.z + s3 * c2.z + s4 * a2.z;

            return new Vector3( x, y, z );
        }

        /// Returns a vector tangent to the point at time 't'
        /// This is the vector tangent to the curve at that point
        public static Vector3 EvaluateCurveDerivative(Vector3[] points, float t)
        {
            Debug.Assert(points.Length == 4, "Incorrect number of points supplied to cubic bezier function (expected 4).");
            if (points.Length >= 4)
            {
                return EvaluateCurveDerivative(points[0], points[1], points[2], points[3], t);
            }
            return Vector3.zero;
        }

        /// Calculates the derivative of the curve at time 't'
        /// This is the vector tangent to the curve at that point
        public static Vector3 EvaluateCurveDerivative(Vector3 a1, Vector3 c1, Vector3 c2, Vector3 a2, float t)
        {
            t = t < 0f ? 0f : t > 1f ? 1f : t;

            float s1 = 3 * (1 - t) * (1 - t);
            float s2 = 6 * (1 - t) * t;
            float s3 = 3 * t * t;

            float x = s1 * (c1.x - a1.x) + s2 * (c2.x - c1.x) + s3 * (a2.x - c2.x);
            float y = s1 * (c1.y - a1.y) + s2 * (c2.y - c1.y) + s3 * (a2.y - c2.y);
            float z = s1 * (c1.z - a1.z) + s2 * (c2.z - c1.z) + s3 * (a2.z - c2.z);

            return new Vector3( x, y, z );
        }

        /// Returns the second derivative of the curve at time 't'
        public static Vector3 EvaluateCurveSecondDerivative(Vector3[] points, float t)
        {
            Debug.Assert(points.Length == 4, "Incorrect number of points supplied to cubic bezier function (expected 4).");
            if (points.Length >= 4)
            {
                return EvaluateCurveSecondDerivative(points[0], points[1], points[2], points[3], t);
            }
            return Vector3.zero;
        }

        ///Returns the second derivative of the curve at time 't'
        public static Vector3 EvaluateCurveSecondDerivative(Vector3 a1, Vector3 c1, Vector3 c2, Vector3 a2, float t)
        {
            t = t < 0f ? 0f : t > 1f ? 1f : t;

            float s1 = 6 * (1 - t);
            float s2 = 6 * t;

            float x = s1 * (c2.x - 2 * c1.x + a1.x) + s2 * (a2.x - 2 * c2.x + c1.x);
            float y = s1 * (c2.y - 2 * c1.y + a1.y) + s2 * (a2.y - 2 * c2.y + c1.y);
            float z = s1 * (c2.z - 2 * c1.z + a1.z) + s2 * (a2.z - 2 * c2.z + c1.z);

            return new Vector3( x, y, z );
        }


        /// Calculates the normal vector (vector perpendicular to the curve) at specified time
        public static Vector3 Normal(Vector3[] points, float t)
        {
            Debug.Assert(points.Length == 4, "Incorrect number of points supplied to cubic bezier function (expected 4).");
            if (points.Length >= 4)
            {
                return Normal(points[0], points[1], points[2], points[3], t);
            }
            return Vector3.zero;
        }

        /// Calculates the normal vector (vector perpendicular to the curve) at specified time
        public static Vector3 Normal(Vector3 a1, Vector3 c1, Vector3 c2, Vector3 a2, float t)
        {
            Vector3 tangent = EvaluateCurveDerivative(a1, c1, c2, a2, t);
            Vector3 nextTangent = EvaluateCurveSecondDerivative(a1, c1, c2, a2, t);
            Vector3 c = Vector3.Cross(nextTangent, tangent);
            return Vector3.Cross(c, tangent).normalized;
        }

        public static Bounds CalculateBounds(Vector3[] points)
        {
            Debug.Assert(points.Length == 4, "Incorrect number of points supplied to cubic bezier function (expected 4)");
            MinMax3D minMax = new MinMax3D();
            if (points.Length >= 4)
            {
                minMax.AddValue(points[0]);
                minMax.AddValue(points[3]);

                List<float> extremePointTimes = ExtremePointTimes(points[0], points[1], points[2], points[3]);
                foreach (float t in extremePointTimes)
                {
                    minMax.AddValue(CubicBezierUtility.EvaluateCurve(points, t));
                }
            }

            return new Bounds((minMax.Min + minMax.Max) / 2, minMax.Max - minMax.Min);
        }

        /// Splits curve into two curves at time t. Returns 2 arrays of 4 points.
        public static Vector3[][] SplitCurve(Vector3[] points, float t)
        {
            Vector3 a1 = Vector3.Lerp(points[0], points[1], t);
            Vector3 a2 = Vector3.Lerp(points[1], points[2], t);
            Vector3 a3 = Vector3.Lerp(points[2], points[3], t);
            Vector3 b1 = Vector3.Lerp(a1, a2, t);
            Vector3 b2 = Vector3.Lerp(a2, a3, t);
            Vector3 pointOnCurve = Vector3.Lerp(b1, b2, t);

            return new Vector3[][] {
                new Vector3[] { points[0], a1, b1, pointOnCurve },
                new Vector3[] { pointOnCurve, b2, a3, points[3] }
            };
        }

        // Crude, but fast estimation of curve length.
        public static float EstimateCurveLength(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            float controlNetLength = (p0 - p1).magnitude + (p1 - p2).magnitude + (p2 - p3).magnitude;
            float estimatedCurveLength = (p0 - p3).magnitude + controlNetLength / 2f;
            return estimatedCurveLength;
        }

        /// Times of stationary points on curve (points where derivative is zero on any axis)
        public static List<float> ExtremePointTimes(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            // coefficients of derivative function
            Vector3 a = 3 * (-p0 + 3 * p1 - 3 * p2 + p3);
            Vector3 b = 6 * (p0 - 2 * p1 + p2);
            Vector3 c = 3 * (p1 - p0);

            List<float> times = new List<float>();
            times.AddRange(StationaryPointTimes(a.x, b.x, c.x));
            times.AddRange(StationaryPointTimes(a.y, b.y, c.y));
            times.AddRange(StationaryPointTimes(a.z, b.z, c.z));
            return times;
        }


        // Finds times of stationary points on curve defined by ax^2 + bx + c.
        // Only times between 0 and 1 are considered as Bezier only uses values in that range
        static IEnumerable<float> StationaryPointTimes(float a, float b, float c)
        {
            List<float> times = new List<float>();

            // from quadratic equation: y = [-b +- sqrt(b^2 - 4ac)]/2a
            if (a != 0)
            {
                float discriminant = b * b - 4 * a * c;
                if (discriminant >= 0)
                {
                    float s = Mathf.Sqrt(discriminant);
                    float t1 = (-b + s) / (2 * a);
                    if (t1 >= 0 && t1 <= 1)
                    {
                        times.Add(t1);
                    }

                    if (discriminant != 0)
                    {
                        float t2 = (-b - s) / (2 * a);

                        if (t2 >= 0 && t2 <= 1)
                        {
                            times.Add(t2);
                        }
                    }
                }
            }
            return times;
        }

    }
}
