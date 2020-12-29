using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace ITnnovative.AOP.Processing.Exectution
{
    public static class Tools
    {

        /// <summary>
        /// Gets method from type using parameters
        /// </summary>
        public static MethodInfo GetMethod(this Type t, string methodName, List<Type> paramTypes)
        {
            var pTypes = paramTypes?.ToArray();
            var method = t.GetMethod(methodName, pTypes);

            if (method == null)
            {
                // Get all methods
                var methods = t.GetMethods((BindingFlags) int.MaxValue);
                foreach (var m in methods)
                {
                    // If name differs, continue
                    if (m.Name != methodName) continue;

                    // Get params 
                    var methodParams = m.GetParameters();
                    for (var index = 0; index < methodParams.Length; index++)
                    {
                        var param = methodParams[index];
                        // Check if params match
                        if (param.ParameterType != pTypes[index])
                        {
                            // Check if is generic-based parameter
                            if (!param.ParameterType.Name.StartsWith("!!"))
                            {
                                // If not, then wrong param is provided, check next method
                                goto continue_job;
                            }
                        }
                    }

                    method = m;
                    break;
                    continue_job: ;
                }

            }

            return method;
        }

    }
}