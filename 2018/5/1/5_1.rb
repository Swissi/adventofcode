def checkPair(current, next_c)
  # check if they are the same character
  # puts "Checking: " + current + " with " + next_c
  return unless current.casecmp(next_c).zero?

  current != next_c
end

@inp_register = ""

def snip(input)
  (0..input.length).each do |i|
    current = input[i]

    return unless i != input.length-1


    next_c = input[i+1]

    next if next_c.nil?
    if checkPair(current, next_c)
      # ok need to cut current and next_c out of string
      @inp_register.slice!(i,2)
      #snip(@inp_register)

    end
  end
end

inp = File.readlines('i.txt')
@inp_register = inp[0]

(0..20000).each do |i|
  snip(@inp_register)
end

puts @inp_register
puts @inp_register.length


