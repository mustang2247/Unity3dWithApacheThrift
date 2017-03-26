/*
 * Licensed to the Apache Software Foundation (ASF) under one
 * or more contributor license agreements. See the NOTICE file
 * distributed with this work for additional information
 * regarding copyright ownership. The ASF licenses this file
 * to you under the Apache License, Version 2.0 (the
 * "License"); you may not use this file except in compliance
 * with the License. You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing,
 * software distributed under the License is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
 * KIND, either express or implied. See the License for the
 * specific language governing permissions and limitations
 * under the License.
 */

using System;
using Thrift;
using Thrift.Protocol;
using Thrift.Server;
using Thrift.Transport;
using UnityEngine;


namespace CSharpTutorial
{
    public class CSharpClient
    {
        public static void Main()
        {
            try
            {
                TTransport transport = new TSocket("localhost", 7777);
                TProtocol protocol = new TBinaryProtocol(transport);

                Calculator.Client client = new Calculator.Client(protocol);

                transport.Open();
                try
                {
                    client.ping();
                    Debug.Log("ping()");

                    int sum = client.add(1, 1);
                    Debug.Log("1+1={0}"+ sum);

                    Work work = new Work();

                    work.Op = Operation.DIVIDE;
                    work.Num1 = 1;
                    work.Num2 = 0;
                    try
                    {
                        int quotient = client.calculate(1, work);
                        Debug.Log("Whoa we can divide by 0");
                    }
                    catch (InvalidOperation io)
                    {
                        Debug.Log("Invalid operation: " + io.Why);
                    }

                    work.Op = Operation.SUBTRACT;
                    work.Num1 = 15;
                    work.Num2 = 10;
                    try
                    {
                        int diff = client.calculate(1, work);
                        Debug.Log("15-10={0}"+ diff);
                    }
                    catch (InvalidOperation io)
                    {
                        Debug.Log("Invalid operation: " + io.Why);
                    }

                    SharedStruct log = client.getStruct(1);
                    Debug.Log("Check log: {0}"+ log.Value);

                }
                finally
                {
                    transport.Close();
                }
            }
            catch (TApplicationException x)
            {
                Debug.Log(x.StackTrace);
            }

        }
    }
}
