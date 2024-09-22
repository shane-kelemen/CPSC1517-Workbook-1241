using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// As with Exercise 01, note that this Ball class has been placed in a separate file, but is still accessible
// by other classes within the same namespace, in this case the Form class.
namespace ClassesReviewPlusMore
{
    // Static - This class is built partly to explore the idea of static fields, properties, constructor, and methods.
    //          Adding the static keyword causes the item to become a shared item across all instances of a particular
    //          class.  Remember that an instance of a class is one object of the class type created using the 'new' keyword.
    //          Access of all static items is accomplished through the data type, such as Console.WriteLine(), where Console
    //          is the data type and WriteLine() is a static method.
    public class Ball
    {
        // Static (shared) fields / data members are accessed through use of static Properties and/or methods, the same as 
        // instance specific fields / data members.  In this way they respect the principle of encapsulation. 
        private static int _extraVelocity = 0;   // An initializer may be used just like with an instance specific field
        private static Random rng;                  // Or initialization may be saved for the static constructor

        // Instance specific fields
        private double _radius;         // Radius of the ball
        private Point _centre;          // Location of the ball in the enclosing space (x and y coordinates)
        private Point _velocity;        // The amount the ball will move in the enclosing space each frame (x and y directions)

        // Static (shared) properties may only access other static properties, methods and static fields / data members.
        // Note:  This property has been written as a manual property, but could also have been an automatic property.
        // Static properties follow the same rules as non-static properties for access specification and hiding.
        // For example, the following has a get that is hidden from external users, but a set that may be modified
        // by an external user.
        public static int ExtraVelocity
        {
            private get 
            { 
                return _extraVelocity; 
            }

            set
            {
                _extraVelocity = value;
            }
        }

        // Instance specific manual property
        // Note that instance specific manual properties are generally backed by an instance
        // specific field / data member. There are different philosophies for dealing with data
        // that is considered invalid for a particular field.  You may either correct the input data
        // as has been done with the Radius property, or you may throw exceptions as has been done
        // with the Centre property.
        public double Radius
        {
            get
            {              
                return _radius;               
            }

            set
            {
                if (value < 1)
                {
                    _radius = 1;
                }
                else if (value > 50)
                {
                    _radius = 50;
                }
                else
                {
                    _radius = value;
                }
            }
        }

        public Point Centre
        {
            get
            {
                return _centre;
            }

            set
            {
                if (value.X < 0 || value.X > 799)
                {
                    throw new ArgumentException("X coordinate out of range!");
                }

                if (value.Y < 0 || value.Y > 599)
                {
                    throw new ArgumentException("Y coordinate out of range!");
                }

                _centre = value;
            }
        }

        // Instance specific automatic property.  Note that the get or set may be made private
        // to make it unavailable to the users of the class.
        public Color Colour { get; set; }

        // The static constructor is not called manually like it is when creating class instances.
        // Instead, it is guaranteed that the runtime will call the static constructor before any static
        // feature of the class is used.  As with all static properties and methods, only other static
        // properties, methods, and fields may be accessed.
        static Ball()
        {
            rng = new Random();
        }
        
        // An instance specific constructor as has been used in previously built classes.
        // Note that the Random object has been used here, even though it is static.  Instance specific
        // items (fields, properties, methods) may be used in instance specific contexts without restriction
        // beyond typical access specification restrictions.
        public Ball(double rad, Point point, Color colour) 
        { 
            // If they exist, remember to use the Properties to initialize the data members instead of assigning
            // values to them directly.
            Radius = rad;
            Centre = point;
            Colour = colour;

            // Generate and assign random x and y velocities to be added to each Ball every rendering frame.
            _velocity = new Point(rng.Next(-20, 21), rng.Next(-20, 21));
        }

        // As has been seen before, the default constructor creates a ball with consistent traits, leveraging
        // the complex constructor above via use of the 'this' keyword.
        public Ball() : this(20, new Point(400, 300), Color.Aqua)
        { 
        }


        // This method is an instance specific method, and so it will be able to access the 'extraVelocity'
        // static field defined above.  it shall be used to augment the velocity of each Ball object.
        public void Move()
        {
            Point temp = new Point(Centre.X + _velocity.X + ExtraVelocity, Centre.Y + _velocity.Y + ExtraVelocity);

            // left boundary
            if (temp.X < Radius)
            {
                temp.X = (int)Radius;
                _velocity.X *= -1;
            }

            // top boundary
            if (temp.Y < Radius)
            {
                temp.Y = (int)Radius;
                _velocity.Y *= -1;
            }

            // right boundary
            if (temp.X >= 800 - Radius)
            {
                temp.X = 800 - (int)Radius - 1;
                _velocity.X *= -1;
            }

            // bottom boundary
            if (temp.Y >= 600 - Radius)
            {
                temp.Y = 600 - (int)Radius - 1;
                _velocity.Y *= -1;
            }

            Centre = temp;            
        }
    }
}
