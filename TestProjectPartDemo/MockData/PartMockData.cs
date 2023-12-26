using PartDetailsDemo.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestProjectPartDemo.MockData
{
    class PartMockData
    {
        public static List<Part> GetParts()
        {
            return new List<Part>
            {
                new Part{
                    PartId=1,
                    PartName="Test1",
                    PartDetails="Test1Details"
                },
                new Part{
                    PartId=2,
                    PartName="Test2",
                    PartDetails="Test2Details"
                },
                new Part{
                    PartId=3,
                    PartName="Test3",
                    PartDetails="Test3Details"
                }

            };
        }       
    }
}
